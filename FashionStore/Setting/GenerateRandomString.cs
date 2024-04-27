using System;

namespace FashionStore.Setting
{
    public class GenerateRandomString
    {
        private static Random random = new Random();
        public static string GenerateRandomStringWithDash(int length)
        {
            const string chars = "0123456789abcdef";
            char[] result = new char[length + 1]; // Tăng 1 độ dài để chứa dấu gạch ngang

            // Tạo chuỗi ngẫu nhiên không có dấu gạch ngang
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            // Chèn dấu gạch ngang vào vị trí ngẫu nhiên
            int dashIndex = random.Next(length);
            result[dashIndex] = '-';
            // Tạo chuỗi từ mảng ký tự và loại bỏ các ký tự null cuối cùng (nếu có)
            return new string(result).TrimEnd('\0');
            
        }
    }
}
