using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data {
    public class AuthRepository : IAuthRepository {
        private readonly DataContext _context;

        public AuthRepository (DataContext context) {
            this._context = context;

        }
        public async Task<User> Login (string username, string passowrd) {
            var user = await _context.Users.Include (p => p.Photos).FirstOrDefaultAsync (x => x.Username == username);
            if (user == null) {
                return null;
            }
            if (!VerifyPasswordHash (passowrd, user.PasswordHash, user.PasswordSalt)) {
                return null;
            }
            return user;
        }

        private bool VerifyPasswordHash (string passowrd, byte[] passwordHash, byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 (passwordSalt)) {
                var computedHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (passowrd));
                for (int i = 0; i < computedHash.Length; i++) {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<User> Register (User user, string passowrd) {
            byte[] passwordHash, passowrdSalt;
            CreatePasswordHash (passowrd, out passwordHash, out passowrdSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passowrdSalt;
            await _context.Users.AddAsync (user);
            await _context.SaveChangesAsync ();
            return user;
        }

        private void CreatePasswordHash (string passowrd, out byte[] passwordHash, out byte[] passowrdSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 ()) {
                passowrdSalt = hmac.Key;
                passwordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (passowrd));
            }

        }

        public async Task<bool> UserExists (string username) {
            if (await _context.Users.AnyAsync (x => x.Username == username)) {
                return true;
            }
            return false;
        }
    }
}