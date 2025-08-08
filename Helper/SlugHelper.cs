using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using QuanLyGameConsole.Models;

namespace QuanLyGameConsole.Helper
{
    public class SlugHelper
    {
        private static readonly Dictionary<char, char> CharacterMap = new()
        {
            // Lowercase
            {'à', 'a'}, {'á', 'a'}, {'ạ', 'a'}, {'ả', 'a'}, {'ã', 'a'},
            {'â', 'a'}, {'ấ', 'a'}, {'ầ', 'a'}, {'ậ', 'a'}, {'ẩ', 'a'}, {'ẫ', 'a'},
            {'ă', 'a'}, {'ắ', 'a'}, {'ằ', 'a'}, {'ặ', 'a'}, {'ẳ', 'a'}, {'ẵ', 'a'},
            {'è', 'e'}, {'é', 'e'}, {'ẹ', 'e'}, {'ẻ', 'e'}, {'ẽ', 'e'},
            {'ê', 'e'}, {'ế', 'e'}, {'ề', 'e'}, {'ệ', 'e'}, {'ể', 'e'}, {'ễ', 'e'},
            {'ì', 'i'}, {'í', 'i'}, {'ị', 'i'}, {'ỉ', 'i'}, {'ĩ', 'i'},
            {'ò', 'o'}, {'ó', 'o'}, {'ọ', 'o'}, {'ỏ', 'o'}, {'õ', 'o'},
            {'ô', 'o'}, {'ố', 'o'}, {'ồ', 'o'}, {'ộ', 'o'}, {'ổ', 'o'}, {'ỗ', 'o'},
            {'ơ', 'o'}, {'ớ', 'o'}, {'ờ', 'o'}, {'ợ', 'o'}, {'ở', 'o'}, {'ỡ', 'o'},
            {'ù', 'u'}, {'ú', 'u'}, {'ụ', 'u'}, {'ủ', 'u'}, {'ũ', 'u'},
            {'ư', 'u'}, {'ứ', 'u'}, {'ừ', 'u'}, {'ự', 'u'}, {'ử', 'u'}, {'ữ', 'u'},
            {'ỳ', 'y'}, {'ý', 'y'}, {'ỵ', 'y'}, {'ỷ', 'y'}, {'ỹ', 'y'},
            {'đ', 'd'},
            
            // Uppercase
            {'À', 'A'}, {'Á', 'A'}, {'Ạ', 'A'}, {'Ả', 'A'}, {'Ã', 'A'},
            {'Â', 'A'}, {'Ấ', 'A'}, {'Ầ', 'A'}, {'Ậ', 'A'}, {'Ẩ', 'A'}, {'Ẫ', 'A'},
            {'Ă', 'A'}, {'Ắ', 'A'}, {'Ằ', 'A'}, {'Ặ', 'A'}, {'Ẳ', 'A'}, {'Ẵ', 'A'},
            {'È', 'E'}, {'É', 'E'}, {'Ẹ', 'E'}, {'Ẻ', 'E'}, {'Ẽ', 'E'},
            {'Ê', 'E'}, {'Ế', 'E'}, {'Ề', 'E'}, {'Ệ', 'E'}, {'Ể', 'E'}, {'Ễ', 'E'},
            {'Ì', 'I'}, {'Í', 'I'}, {'Ị', 'I'}, {'Ỉ', 'I'}, {'Ĩ', 'I'},
            {'Ò', 'O'}, {'Ó', 'O'}, {'Ọ', 'O'}, {'Ỏ', 'O'}, {'Õ', 'O'},
            {'Ô', 'O'}, {'Ố', 'O'}, {'Ồ', 'O'}, {'Ộ', 'O'}, {'Ổ', 'O'}, {'Ỗ', 'O'},
            {'Ơ', 'O'}, {'Ớ', 'O'}, {'Ờ', 'O'}, {'Ợ', 'O'}, {'Ở', 'O'}, {'Ỡ', 'O'},
            {'Ù', 'U'}, {'Ú', 'U'}, {'Ụ', 'U'}, {'Ủ', 'U'}, {'Ũ', 'U'},
            {'Ư', 'U'}, {'Ứ', 'U'}, {'Ừ', 'U'}, {'Ự', 'U'}, {'Ử', 'U'}, {'Ữ', 'U'},
            {'Ỳ', 'Y'}, {'Ý', 'Y'}, {'Ỵ', 'Y'}, {'Ỷ', 'Y'}, {'Ỹ', 'Y'},
            {'Đ', 'D'}
        };

        public static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return string.Empty;

            // Convert Vietnamese characters to Latin
            string str = ConvertToLatinChars(phrase);

            // Convert to lowercase
            str = str.ToLower();

            // Remove invalid chars
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // Remove multiple spaces
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // Cut and trim
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();

            // Replace spaces with hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        private static string ConvertToLatinChars(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var characterMap = new Dictionary<string, string>
            {
                { "á", "a" }, { "à", "a" }, { "ả", "a" }, { "ã", "a" }, { "ạ", "a" },
                { "â", "a" }, { "ấ", "a" }, { "ầ", "a" }, { "ẩ", "a" }, { "ẫ", "a" }, { "ậ", "a" },
                { "ă", "a" }, { "ắ", "a" }, { "ằ", "a" }, { "ẳ", "a" }, { "ẵ", "a" }, { "ặ", "a" },
                { "đ", "d" }, { "é", "e" }, { "è", "e" }, { "ẻ", "e" }, { "ẽ", "e" }, { "ẹ", "e" },
                { "ê", "e" }, { "ế", "e" }, { "ề", "e" }, { "ể", "e" }, { "ễ", "e" }, { "ệ", "e" },
                { "í", "i" }, { "ì", "i" }, { "ỉ", "i" }, { "ĩ", "i" }, { "ị", "i" },
                { "ó", "o" }, { "ò", "o" }, { "ỏ", "o" }, { "õ", "o" }, { "ọ", "o" },
                { "ô", "o" }, { "ố", "o" }, { "ồ", "o" }, { "ổ", "o" }, { "ỗ", "o" }, { "ộ", "o" },
                { "ơ", "o" }, { "ớ", "o" }, { "ờ", "o" }, { "ở", "o" }, { "ỡ", "o" }, { "ợ", "o" },
                { "ú", "u" }, { "ù", "u" }, { "ủ", "u" }, { "ũ", "u" }, { "ụ", "u" },
                { "ư", "u" }, { "ứ", "u" }, { "ừ", "u" }, { "ử", "u" }, { "ữ", "u" }, { "ự", "u" },
                { "ý", "y" }, { "ỳ", "y" }, { "ỷ", "y" }, { "ỹ", "y" }, { "ỵ", "y" },
                { "Đ", "D" }
            };

            foreach (var kvp in characterMap)
            {
                text = text.Replace(kvp.Key, kvp.Value);
            }

            return text;
        }


        public enum EntityType
        {
            Product,
            Category
        }

        public static async Task<string> GenerateUniqueSlug(GameConsoleContext context, string name, EntityType entityType, int? entityId = null)
        {

            // Tạo slug từ tên
            string slug = GenerateSlug(name);

            // Kiểm tra nếu slug đã tồn tại trong cơ sở dữ liệu
            if (entityType == EntityType.Product)
            {
                var existingProduct = await context.Products
                    .AsNoTracking() // Chỉ lấy dữ liệu mà không giữ trạng thái theo dõi
                    .Where(p => p.Slug == slug && (entityId == null || p.ProductId != entityId))
                    .FirstOrDefaultAsync();

                // Nếu có sản phẩm trùng slug, thêm Id vào cuối slug hoặc tạo số đếm duy nhất
                if (existingProduct != null)
                {
                    int counter = 1;
                    string originalSlug = slug;

                    // Kiểm tra và tạo slug duy nhất
                    while (await context.Products.AnyAsync(p => p.Slug == slug && (entityId == null || p.ProductId != entityId)))
                    {
                        slug = $"{originalSlug}-{counter}";
                        counter++;
                    }
                }
            }
            else if (entityType == EntityType.Category)
            {
                var existingCategory = await context.Categories
                    .AsNoTracking() // Chỉ lấy dữ liệu mà không giữ trạng thái theo dõi
                    .Where(c => c.Slug == slug && (entityId == null || c.CategoryId != entityId))
                    .FirstOrDefaultAsync();

                // Nếu có category trùng slug, thêm Id vào cuối slug hoặc tạo số đếm duy nhất
                if (existingCategory != null)
                {
                    int counter = 1;
                    string originalSlug = slug;

                    // Kiểm tra và tạo slug duy nhất
                    while (await context.Categories.AnyAsync(c => c.Slug == slug && (entityId == null || c.CategoryId != entityId)))
                    {
                        slug = $"{originalSlug}-{counter}";
                        counter++;
                    }
                }
            }

            return slug;
        }

    }
}