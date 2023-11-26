using Reviews_AJAX.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Reviews_AJAX.Repos
{
    public class ReviewsRepository : IRepository
    {
        private readonly ReviewContext _context;
        public ReviewsRepository(ReviewContext context)
        {
            _context = context;
        }

        public async Task<List<UserReviewVM>> GetReviews()
        {
            var col = await _context.Reviews.ToListAsync();
            List<UserReviewVM> res = new();
            foreach (var item in col)
            {
                var Uuser = await _context.Users.FindAsync(item.UserId);
                UserReviewVM appitem = new()
                {
                    UserLogin = Uuser.Login,
                    ReviewText = item.ReviewText,
                    ReviewDate = item.ReviewDate
                };
                res.Add(appitem);
            }
            return res;
        }
        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> FindUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task CreateReview(UserReviewVM reviewVM)
        {
            var uuser = await _context.Users.FirstOrDefaultAsync(x => x.Login == reviewVM.UserLogin);
            Review review = new Review
            {
                UserId = uuser.Id,
                ReviewText = reviewVM.ReviewText,
                ReviewDate = reviewVM.ReviewDate
            };
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> TryToRegister(RegisterVM registerVM)
        {
            var IsFinded = await _context.Users.FirstOrDefaultAsync(x => x.Login == registerVM.Login);
            // Якщо такого користувача знайдено, значить, такий логін вже зайнято
            if (IsFinded != null)
            {
                return false;
            }
            // Якщо такого користувача немає - зареєструвати і повернути true
            else
            {
                User user = new User();
                user.FirstName = registerVM.FirstName;
                user.LastName = registerVM.LastName;
                user.Login = registerVM.Login;

                byte[] saltbuf = new byte[16];

                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + registerVM.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Password = hash.ToString();
                user.Salt = salt;
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
        }
        public async Task<string?> TryToLogin(LoginVM loginVM)
        {
            if (_context.Users.Count() == 0) return null;
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Login.Equals(loginVM.Login));
            if (user == null)
            {
                return null;
            }
            else
            {
                string? salt = user.Salt;

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + loginVM.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (user.Password != hash.ToString())
                {
                    return null;
                }
                return $"{user.FirstName} {user.LastName}";
            }
        }
    }
}
