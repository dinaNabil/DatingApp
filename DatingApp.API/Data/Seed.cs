using System.Collections.Generic;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data {
    public class Seed {
        private readonly DataContext _context;

        public Seed (DataContext context) {
            _context = context;
        }
        private void CreatePasswordHash (string passowrd, out byte[] passwordHash, out byte[] passowrdSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 ()) {
                passowrdSalt = hmac.Key;
                passwordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (passowrd));
            }

        }
        public void SeedUsers () {
            var userData = System.IO.File.ReadAllText ("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>> (userData);
            foreach (var user in users) {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("password",out passwordHash,out passwordSalt);
                user.PasswordHash=passwordHash;
                user.PasswordSalt=passwordSalt;
                user.Username=user.Username.ToLower();
                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }
    }
}