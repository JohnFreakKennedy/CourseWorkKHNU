using System;
using System.Numerics;
using System.Threading;

namespace MyCryptography
{
    public class Fastel_Cr
    {
        public byte[] CryptFastelAutonom(byte[] data, bool cl, byte[] k, bool ran = true)
        {
            byte d;
            byte add;
            if (cl)
            {
                add = 0;
                NumberGen rand = new NumberGen();
                Array.Resize(ref data, data.Length + 1);
                data[data.Length - 1] = data[0];
                while (data.Length % 3 != 0)
                {
                    Array.Resize(ref data, data.Length + 1);
                    if (ran)
                        data[data.Length - 1] = (byte)rand.Generate(256);
                    else
                        data[data.Length - 1] = data[0];
                    add++;
                }
                data[data.Length - 1] = add;
            }

            else
            {
                k = KeyReverse(k);
            }
            int fun;
            byte[] ret = new byte[3];
            for (int i = 0; i <= data.Length - 1; i += 3)
            {
                for (int j = 0; j <= 15; j++)
                {
                    fun = (int)f(data[i], k[j]);
                    ret[2] = data[i];
                    ret[0] = (byte)((int)data[i + 1] ^ fun);
                    ret[1] = (byte)((int)data[i + 2] ^ fun);
                    data[i] = ret[0];
                    data[i + 1] = ret[1];
                    data[i + 2] = ret[2];
                }
                d = data[i];
                data[i] = data[i + 2];
                data[i + 2] = d;
            }
            if (!cl)
            {
                add = data[data.Length - 1];
                Array.Resize(ref data, data.Length - ((int)add + 1));
            }
            return data;
        }
        byte[] KeyReverse(byte[] k_old)
        {
            byte[] k_new = new byte[k_old.Length];
            for (int i = 0, j = k_old.Length - 1; i <= k_old.Length - 1; i++, j--)
                k_new[i] = k_old[j];
            return k_new;
        }
        private byte f(byte b, byte k)
        {
            return (byte)((int)b ^ (int)k);
        }
        public byte[] XOR(byte[] b1, byte[] b2)
        {
            byte[] result = new byte[b1.Length];
            for (int i = 0; i < b1.Length; i++)
            {
                result[i] = (byte)(((int)b1[i] ^ (int)b2[i]));
            }
            return result;
        }
        public byte[] Triple_crypt(byte[] data, bool cl, byte[] k)
        {
            byte[] cr = null, key1 = new byte[16], key2 = new byte[16], key3 = new byte[16];
            int i = (int)k[0];
            Array.Resize(ref k, 48);
            Array.Copy(k, 0, key1, 0, 16);
            Array.Copy(k, 16, key2, 0, 16);
            Array.Copy(k, 32, key3, 0, 16);
            byte add;
            if (cl)
            {
                add = 0;
                NumberGen rand = new NumberGen();
                Array.Resize(ref data, data.Length + 1);
                data[data.Length - 1] = (byte)rand.Generate(256);
                while (data.Length % 3 != 0)
                {
                    Array.Resize(ref data, data.Length + 1);
                    data[data.Length - 1] = (byte)rand.Generate(256);
                    add++;
                }
                data[data.Length - 1] = add;
                cr = CryptFastel1(data, true, key1);
                cr = CryptFastel1(cr, false, key2);
                cr = CryptFastel1(cr, true, key3);
            }
            else
            {
                cr = CryptFastel1(data, false, key3);
                cr = CryptFastel1(cr, true, key2);
                cr = CryptFastel1(cr, false, key1);
                add = data[data.Length - 1];
                Array.Resize(ref cr, cr.Length - ((int)add + 1));
            }
            return cr;
        }
        public byte[] CryptFastel1(byte[] data, bool cl, byte[] k)
        {
            byte d;
            if (!cl)
            {
                k = KeyReverse(k);
            }
            int fun;
            byte[] ret = new byte[3];
            for (int i = 0; i <= data.Length - 1; i += 3)
            {
                for (int j = 0; j <= 15; j++)
                {
                    fun = (int)f(data[i], k[j]);
                    ret[2] = data[i];
                    ret[0] = (byte)((int)data[i + 1] ^ fun);
                    ret[1] = (byte)((int)data[i + 2] ^ fun);
                    data[i] = ret[0];
                    data[i + 1] = ret[1];
                    data[i + 2] = ret[2];
                }
                d = data[i];
                data[i] = data[i + 2];
                data[i + 2] = d;
            }
            return data;
        }
        public byte[] KeyCorrect(byte[] key)
        {
            byte[][] Sbox_key = new byte[16][];
            Sbox_key[0] = new byte[] { 13, 7, 4, 3, 15, 0, 1, 9, 12, 6, 8, 2, 10, 5, 14, 11 };
            Sbox_key[1] = new byte[] { 12, 9, 14, 13, 8, 10, 15, 2, 11, 7, 5, 0, 6, 1, 3, 4 };
            Sbox_key[2] = new byte[] { 1, 10, 4, 12, 5, 13, 9, 14, 3, 2, 6, 7, 11, 15, 0, 8 };
            Sbox_key[3] = new byte[] { 11, 13, 6, 0, 3, 2, 12, 4, 14, 9, 1, 7, 10, 8, 5, 15 };
            Sbox_key[4] = new byte[] { 15, 12, 11, 6, 10, 5, 9, 0, 1, 3, 7, 4, 2, 13, 8, 14 };
            Sbox_key[5] = new byte[] { 2, 5, 0, 6, 15, 14, 10, 9, 3, 11, 8, 4, 13, 1, 12, 7 };
            Sbox_key[6] = new byte[] { 3, 13, 15, 6, 8, 9, 0, 12, 7, 2, 10, 11, 14, 5, 1, 4 };
            Sbox_key[7] = new byte[] { 3, 1, 7, 0, 10, 13, 15, 11, 12, 6, 8, 4, 5, 14, 2, 9 };
            Sbox_key[8] = new byte[] { 15, 7, 3, 0, 6, 1, 12, 11, 8, 14, 5, 10, 9, 13, 2, 4 };
            Sbox_key[9] = new byte[] { 14, 7, 2, 6, 8, 9, 1, 0, 13, 11, 3, 5, 15, 12, 10, 4 };
            Sbox_key[10] = new byte[] { 10, 3, 6, 8, 11, 12, 14, 15, 9, 2, 7, 4, 5, 1, 0, 13 };
            Sbox_key[11] = new byte[] { 0, 13, 15, 10, 4, 12, 7, 9, 8, 14, 6, 11, 3, 1, 5, 2 };
            Sbox_key[12] = new byte[] { 9, 12, 10, 7, 5, 3, 14, 4, 15, 6, 8, 11, 1, 13, 2, 0 };
            Sbox_key[13] = new byte[] { 9, 7, 2, 15, 8, 5, 3, 1, 12, 0, 14, 10, 6, 11, 13, 4 };
            Sbox_key[14] = new byte[] { 8, 2, 13, 3, 7, 6, 15, 10, 14, 4, 0, 1, 9, 12, 5, 11 };
            Sbox_key[15] = new byte[] { 15, 1, 2, 6, 0, 11, 12, 4, 9, 7, 10, 13, 8, 3, 5, 14 };
            string bits = "", bits_res;
            byte[] k1 = key; // Формуємо ключ для кінцевого XOR
            while (k1.Length < 48)
            {
                int ind = k1.Length;
                Array.Resize(ref k1, k1.Length + key.Length);
                Array.Copy(key, 0, k1, ind, key.Length);
            }
            Array.Resize(ref k1, 48); // Обрізаємо 48 байт
            for (int i = 0; i <= key.Length - 1; i++)
                bits += Convert.ToString(key[i], 2);
            while (bits.Length < 768) // Виконуємо основне розширення
            {
                while (bits.Length % 4 != 0)
                {
                    bits += bits[0];
                }
                // Виконуємо основний етап розширення
                bits_res = "" + bits[bits.Length - 2] + bits[0] + bits[1] + bits[2] +
                bits[3] + bits[5]; // Перша тетрада
                for (int i = 1, j = 4; i <= (bits.Length / 4) - 1; i++, j += 4)
                {
                    bits_res += "" + bits_res[bits_res.Length - 3] + bits[j] + bits[j +
                   1] + bits[j + 2] + bits[j + 3];
                    if ((j + 5) > bits.Length) // Остання тетрада
                        bits_res += bits_res[2];
                    else
                        bits_res += bits[j + 5];
                }
                bits = bits_res;
            }
            bits_res = "";
            for (int i = 0; i < bits.Length / 8; i++) // Виконуємо заміну по S-box
            {
                int bt_r = Convert.ToInt32("" + bits[i * 8 + 2] + bits[i * 8 + 4] +
               bits[i * 8 + 5]
                + bits[i * 8 + 7], 2);
                int bt_c = Convert.ToInt32("" + bits[i * 8] + bits[i * 8 + 1] + bits[i
               * 8 + 3] + bits[i * 8 + 6], 2);
                string k = Convert.ToString(Sbox_key[bt_r][bt_c], 2);
                while (k.Length % 4 != 0)
                    k = "0" + k;
                bits_res += k;
            }
            key = new byte[(bits_res.Length / 8)];
            for (int i = 0; i < bits_res.Length / 8; i++)
                key[i] = Convert.ToByte("" + bits[i * 8] + bits[i * 8 + 1] + bits[i *
               8 + 2] +
                bits[i * 8 + 3] + bits[i * 8 + 4] + bits[i * 8 + 5] + bits[i * 8 + 6] +
               bits[i * 8 + 7], 2);
            Array.Resize(ref key, 48); // Обрізаємо 48 байт
            key = Shift(key);
            key = XOR(k1, key);
            return key;
        }
        private byte[] Shift(byte[] b)
        {
            byte[] key_buf = new byte[b.Length];
            key_buf[0] = b[b.Length - 1];
            key_buf[1] = b[2];
            for (int n = 2; n <= b.Length - 1; n += 2)
            {
                key_buf[n] = b[n - 1];
                if ((n + 2) >= b.Length)
                {
                    key_buf[n + 1] = b[0];
                    break;
                }
                key_buf[n + 1] = b[n + 2];
            }
            return key_buf;
        }
        public string Encode(byte[] text)
        {
            byte[] open = new byte[128];
            for (int i = 0; i < 128; i++)
            {
                open[i] = Convert.ToByte(i);
            }
            char[] ch = new char[] { 'k', 'l', 'm', 'Щ', 'c', 'd', 'F', 'W', 'X', 'н', 'о', 'п',
'Ь', 'Ю', 'Т', 'У', 'Ф', 'и', 'й', 'к', 'є', 'Н', 'ц', 'у', 'ф', 'х', 'Г', 'Р', 'в', 'Й', 'К', 'Л', 'a',
'b', 'р', 'с', 'т', 'Е', 'М', 'С', 'е', 'ж', 'з', 'O', 'P', 'Q', 'R', 'v', 'w', 'x', 'B', 'U', 'V', '9',
'л', 'м', 'D', 'E', '5', '6', '7', '8', 'G', 'Б', 'y', 'z', 'A', 'В', 'T', 'і', 'ы', '3', 'H', 'э', 'I', 'J',
'K', 'L', 'M', 'Ы', 'N', 'n', 'o', 'я', '1', '2', 'О', 'П', 'д', 'Я', 'а', 'б', 'C', 'p', 'І', 'q', 'r',
's', 'ш', 'Ж', 'S', 'Х', 'Ц', 'Ч', 'Д', 'ч', 'г', 'Y', 'Z', 'А', 'щ', 'ь', 't', 'u', 'З', 'И', 'ю', 'Ш',
'h', 'i', 'j', '0', '4', 'e', 'f', 'g', 'Э', 'Є' };
            string code = "";
            for (int i = 0; i < text.Length; i++)
            {
                string c = Convert.ToString(text[i], 2);
                while (c.Length % 8 != 0)
                    c = "0" + c;
                code += c;
            }
            while (code.Length % 7 != 0)
                code += "0";
            char[] cd = code.ToCharArray();
            string[] bits = new string[code.Length / 7];
            for (int i = 0, j = 0; i < cd.Length; i += 7, j++)
            {
                bits[j] = "" + cd[i] + cd[i + 1] + cd[i + 2] + cd[i + 3] + cd[i + 4] +
               cd[i + 5] + cd[i + 6];
            }
            int[] b_int = new int[bits.Length];
            for (int i = 0; i < b_int.Length; i++)
            {
                b_int[i] = Convert.ToInt32(bits[i], 2);
            }
            string result = "";
            for (int j = 0; j < b_int.Length; j++)
            {
                int ctrl = 0, i = 0;
                for (; i < 128; i++)
                {
                    int op = open[i];
                    if (op == b_int[j])
                    {
                        ctrl = 1;
                        break;
                    }
                }
                if (ctrl == 0) return null;
                result += ch[i];
            }
            return result;
        }
        public byte[] Decode(string text)
        {
            byte[] open = new byte[128];
            for (int i = 0; i < 128; i++)
            {
                open[i] = Convert.ToByte(i);
            }
            char[] ch = new char[] { 'k', 'l', 'm', 'Щ', 'c', 'd', 'F', 'W', 'X', 'н', 'о', 'п',
'Ь', 'Ю', 'Т', 'У', 'Ф', 'и', 'й', 'к', 'є', 'Н', 'ц', 'у', 'ф', 'х', 'Г', 'Р', 'в', 'Й', 'К', 'Л', 'a',
'b', 'р', 'с', 'т', 'Е', 'М', 'С', 'е', 'ж', 'з', 'O', 'P', 'Q', 'R', 'v', 'w', 'x', 'B', 'U', 'V', '9',
'л', 'м', 'D', 'E', '5', '6', '7', '8', 'G', 'Б', 'y', 'z', 'A', 'В', 'T', 'і', 'ы', '3', 'H', 'э', 'I', 'J',
'K', 'L', 'M', 'Ы', 'N', 'n', 'o', 'я', '1', '2', 'О', 'П', 'д', 'Я', 'а', 'б', 'C', 'p', 'І', 'q', 'r',
's', 'ш', 'Ж', 'S', 'Х', 'Ц', 'Ч', 'Д', 'ч', 'г', 'Y', 'Z', 'А', 'щ', 'ь', 't', 'u', 'З', 'И', 'ю', 'Ш',
'h', 'i', 'j', '0', '4', 'e', 'f', 'g', 'Э', 'Є' };
            string bits_st = "";
            for (int i = 0; i < text.Length; i++)
            {
                int ctrl = 0, j = 0;
                for (; j < 128; j++)
                {
                    if (ch[j] == text[i])
                    {
                        ctrl = 1;
                        break;
                    }
                }
                if (ctrl == 0) return null;
                string bit = Convert.ToString(open[j], 2);
                while (bit.Length % 7 != 0)
                    bit = "0" + bit;
                bits_st += bit;
            }
            byte[] bits = new byte[bits_st.Length / 8];
            for (int i = 0, j = 0; i < bits_st.Length; i += 8, j++)
            {
                try
                {
                    string bt = "" + bits_st[i] + bits_st[i + 1] + bits_st[i + 2] + bits_st[i + 3] + bits_st[i + 4] + bits_st[i + 5] + bits_st[i + 6] + bits_st[i + 7];
                    int bt_int = Convert.ToInt32(bt, 2);
                    bits[j] = (byte)bt_int;
                }
                catch
                {
                    break;
                }
            }
            return bits;
        }
    }
    public class NumberGen
    {
        //генератор випадкових чисел
        private readonly Random _random = new Random();
        public ulong Generate(ulong max)
        {
            var Bytes = new byte[4];
            _random.NextBytes(Bytes);
            ulong number = BitConverter.ToUInt32(Bytes, 0);
            number = number % max;
            return number;
        }
    }
    public static class PrimeExtensions
    {
        // Random generator (thread safe)
        private static ThreadLocal<Random> s_Gen = new
        ThreadLocal<Random>(() =>
        {
            return new Random();
        }
        );
        // Random generator (thread safe)
        private static Random Gen
        {
            get
            {
                return s_Gen.Value;
            }
        }
        public static bool IsProbablyPrime(this BigInteger value, int witnesses = 10)
        {
            if (value <= 1)
                return false;
            if (witnesses <= 0)
                witnesses = 10;
            BigInteger d = value - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }
            Byte[] bytes = new Byte[value.ToByteArray().LongLength];
            BigInteger a;
            for (int i = 0; i < witnesses; i++)
            {
                do
                {
                    Gen.NextBytes(bytes);
                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= value - 2);
                BigInteger x = BigInteger.ModPow(a, d, value);
                if (x == 1 || x == value - 1)
                    continue;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, value);
                    if (x == 1)
                        return false;
                    if (x == value - 1)
                        break;
                }
                if (x != value - 1)
                    return false;
            }
            return true;
        }
    }
    public class PrimeNumberGenerator
    {
        private readonly Random _random = new Random();
        public ulong Generate()
        {
            var numberBytes = new byte[4];
            _random.NextBytes(numberBytes);
            ulong number = BitConverter.ToUInt32(numberBytes, 0);
            while (!IsPrime(number))
            {
                unchecked
                {
                    number++;
                }
            }
            return number;
        }

        private static bool IsPrime(ulong number)
        {
            if ((number & 1) == 0) return (number == 2);

            var limit = (ulong)Math.Sqrt(number);
            for (ulong i = 3; i <= limit; i += 2)
            {
                if ((number % i) == 0) return false;
            }
            return true;
        }
        public BigInteger Generate(BigInteger mask, BigInteger min)
        {
            mask = BigInteger.Pow(mask, 5);
            min = BigInteger.Pow(min, 5);
            var numberBytes = new byte[40];
            BigInteger number;
            do
            {
                _random.NextBytes(numberBytes);
                number = new BigInteger(numberBytes);
                number = number % mask;
            } while (number < min);
            while (!PrimeExtensions.IsProbablyPrime(number))
            {
                unchecked
                {
                    number++;
                }
            }
            return number;
        }

    }
    public class Hesh
    {
        private byte[][] S_Box = new byte[4][];
        private byte[] ret = new byte[3];
        public byte[] GetMyHesh(byte[] data)
        {

            byte[] Hesh = new byte[] { 250, 31, 91, 10, 245, 9, 123, 30, 99, 124, 78, 63, 218, 79, 81, 163 };

            while (data.Length % 16 != 0)
            {
                Array.Resize(ref data, data.Length + 1);
            }
            byte[][] data_block = new byte[data.Length / 16][];
            for (int i = 0, j = 0; i <= data_block.Length - 1; i++)
            {
                data_block[i] = new byte[16];
                for (int k = 0; k <= 15; j++, k++)
                {
                    data_block[i][k] = data[j];
                }
            }
            byte[] Hesh_r;
            for (int i = 0; i <= data_block.Length - 1; i++)
            {
                Hesh_r = Hesh;
                Hesh = XOR(Hesh, data_block[i]);
                Hesh = CryptFastelAutonom(Hesh, true, data_block[i], false);
                Hesh = XOR(Hesh_r, Hesh);
            }
            return Hesh;
        }
        public byte[] GetMyHesh64(byte[] data)
        {
            byte[] Hesh = new byte[] { 178, 28, 201, 109, 38, 251, 142, 61 };
            while (data.Length % 8 != 0)
                Array.Resize(ref data, data.Length + 1);
            byte[][] data_block = new byte[data.Length / 8][];
            for (int i = 0, j = 0; i <= data_block.Length - 1; i++)
            {
                data_block[i] = new byte[8];
                for (int k = 0; k <= 7; j++, k++)
                {
                    data_block[i][k] = data[j];
                }
            }
            byte[] Hesh_r;
            for (int i = 0; i <= data_block.Length - 1; i++)
            {
                Hesh_r = Hesh;
                Hesh = XOR(Hesh, data_block[i]);
                Hesh = CryptHesh(Hesh, data_block[i]);
                Hesh = XOR(Hesh_r, Hesh);
            }
            return Hesh;
        }
        public byte[] CryptHesh(byte[] data, byte[] k)
        {
            byte d;
            while (data.Length % 3 != 0)
            {
                Array.Resize(ref data, data.Length + 1);
                data[data.Length - 1] = data[0];
            }
            S_Box[0] = new byte[] { 6, 11, 3, 15, 9, 0, 10, 2, 5, 8, 12, 14, 1, 13, 7, 4 };
            S_Box[1] = new byte[] { 1, 7, 5, 3, 8, 11, 2, 13, 14, 10, 4, 0, 12, 15, 9, 6 };
            S_Box[2] = new byte[] { 12, 3, 5, 8, 6, 10, 2, 7, 13, 14, 11, 9, 0, 1, 4, 15 };
            S_Box[3] = new byte[] { 2, 7, 3, 5, 12, 15, 13, 1, 8, 0, 10, 14, 6, 11, 9, 4 };
            int fun;
            for (int i = 0; i <= data.Length - 1; i += 3)
            {
                for (int j = 0; j <= k.Length - 1; j++)
                {
                    fun = (int)f(data[i], k[j]);
                    ret[2] = data[i];
                    ret[0] = (byte)((int)data[i + 1] ^ fun);
                    ret[1] = (byte)((int)data[i + 2] ^ fun);
                    data[i] = ret[0];
                    data[i + 1] = ret[1];
                    data[i + 2] = ret[2];
                }
                d = data[i];
                data[i] = data[i + 2];
                data[i + 2] = d;
            }
            return data;
        }
        private byte[] XOR(byte[] b1, byte[] b2)
        {
            byte[] result = new byte[b1.Length];
            for (int i = 0; i < b1.Length; i++)
            {
                result[i] = (byte)(((int)b1[i] ^ (int)b2[i]));
            }
            return result;
        }
        private byte f(byte b, byte k)
        {
            return (byte)((int)b ^ (int)k);
        }
        public byte[] CryptFastelAutonom(byte[] data, bool cl, byte[] k, bool ran = true)
        {
            byte d;
            byte add;
            if (cl)
            {
                add = 0;
                NumberGen rand = new NumberGen();
                Array.Resize(ref data, data.Length + 1);
                data[data.Length - 1] = data[0];
                while (data.Length % 3 != 0)
                {
                    Array.Resize(ref data, data.Length + 1);
                    if (ran)
                        data[data.Length - 1] = (byte)rand.Generate(256);
                    else
                        data[data.Length - 1] = data[0];
                    add++;
                }
                data[data.Length - 1] = add;
            }
            else
            {
                k = KeyReverse(k);
            }
            S_Box[0] = new byte[] { 6, 11, 3, 15, 9, 0, 10, 2, 5, 8, 12, 14, 1, 13, 7, 4 };
            S_Box[1] = new byte[] { 1, 7, 5, 3, 8, 11, 2, 13, 14, 10, 4, 0, 12, 15, 9, 6 };
            S_Box[2] = new byte[] { 12, 3, 5, 8, 6, 10, 2, 7, 13, 14, 11, 9, 0, 1, 4, 15 };
            S_Box[3] = new byte[] { 2, 7, 3, 5, 12, 15, 13, 1, 8, 0, 10, 14, 6, 11, 9, 4 };
            int fun;
            for (int i = 0; i <= data.Length - 1; i += 3)
            {
                for (int j = 0; j <= 15; j++)
                {
                    fun = (int)f(data[i], k[j]);
                    ret[2] = data[i];
                    ret[0] = (byte)((int)data[i + 1] ^ fun);
                    ret[1] = (byte)((int)data[i + 2] ^ fun);
                    data[i] = ret[0];
                    data[i + 1] = ret[1];
                    data[i + 2] = ret[2];
                }
                d = data[i];
                data[i] = data[i + 2];
                data[i + 2] = d;
            }
            if (!cl)
            {
                add = data[data.Length - 1];
                Array.Resize(ref data, data.Length - ((int)add + 1));
            }
            return data;
        }
        byte[] KeyReverse(byte[] k_old)
        {
            byte[] k_new = new byte[k_old.Length];
            for (int i = 0, j = k_old.Length - 1; i <= k_old.Length - 1; i++, j--)
                k_new[i] = k_old[j];
            return k_new;
        }
    }
    public class RSA
    {
        BigInteger[] blocks;
        // Генерація ключів
        public BigInteger PublicKey_PrivateKey(BigInteger p, BigInteger q, out BigInteger e, out BigInteger d)
        {
            Random random = new Random();
            BigInteger n_;
            BigInteger feul;
            d = 0;
            n_ = p * q;
            feul = (p - 1) * (q - 1);
            e = 65537;
            d = invmod(e, feul);
            return n_;
        }
        // Розширений алгоритм Евкліда
        BigInteger invmod(BigInteger a, BigInteger m)
        {
            BigInteger x = 0, y = 0;
            gcdex(a, m, ref x, ref y);
            x = (x % m + m) % m;
            return x;
        }
        BigInteger gcdex(BigInteger a, BigInteger b, ref BigInteger x, ref BigInteger y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
            BigInteger x1 = 0, y1 = 0;
            BigInteger d = gcdex(b % a, a, ref x1, ref y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }
        // Обчислення степені по модулю
        private BigInteger powRSA(BigInteger byt, BigInteger de_, BigInteger n_)
        {
            return BigInteger.ModPow(byt, de_, n_);
        }
        // Функція поділу на блоки відкритого тексту та перемішування його з випадковими байтами
        private bool RetBlocks(byte[] data, BigInteger n_)
        {
            int blockSize = n_.ToString("X").Length / 3;
            while (blockSize % 4 != 0)
                blockSize++;
            string block = "";
            char[] hex = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            Random random = new Random();
            string buf = "";
            int size = ((data.Length * 4) / blockSize);
            if ((data.Length * 4) % blockSize != 0)
                size++;
            blocks = new BigInteger[size];
            for (int i = 0, k = 0; i < data.Length; i++)
            {
                block += hex[random.Next(0, 16)];
                buf = data[i].ToString("X");
                if (buf.Length == 1)
                    buf = "0" + buf;
                block += buf;
                block += hex[random.Next(0, 16)];
                if (i == data.Length - 1)
                {
                    int cl = 8;
                    if (block.Length >= blockSize)
                    {
                        block = block.Substring(0, block.Length - 1);
                        block += "A";
                    }
                    else
                    {
                        if (block.Length + 4 >= blockSize)
                        {
                            block += hex[random.Next(0, 16)];
                            block += hex[random.Next(0, 16)];
                            block += hex[random.Next(0, 16)];
                            block += "B";
                        }
                        else
                        {
                            while (block.Length + 8 < blockSize)
                            {
                                block += hex[random.Next(0, 16)];
                                cl++;
                            }
                            block = block.Substring(0, block.Length - 1);
                            string num = Convert.ToString(cl, 16);
                            while (num.Length < 8)
                                num = "0" + num;
                            block += num + "C";
                        }
                    }
                }
                if (block.Length >= blockSize - 1)
                {
                    blocks[k] = BigInteger.Parse("0" + block,
                   System.Globalization.NumberStyles.AllowHexSpecifier);
                    block = "";
                    k++;
                }
            }
            return true;
        }
        // Функція склеювання блоків розшифрованого тексту та відсіювання випадкових байт
        private byte[] readBlocks(BigInteger n_)
        {
            if (blocks == null)
                return null;
            int blockSize = n_.ToString("X").Length / 3;
            while (blockSize % 4 != 0)
                blockSize++;
            byte[] res = new byte[0];
            string block = "";
            for (int i = 0; i < blocks.Length; i++)
            {
                block = blocks[i].ToString("X");
                if (block.Length == 57)
                    block = block.Substring(1, block.Length - 1);
                while (block.Length % blockSize != 0)
                    block = "0" + block;
                if (i == blocks.Length - 1)
                {
                    if (block[block.Length - 1] == 'A' || block[block.Length - 1] == 'B' || block[block.Length - 1] == 'C')
                    {
                        if (block[block.Length - 1] == 'B')
                        {
                            block = block.Substring(0, block.Length - 4);
                        }
                        else
                        {
                            if (block[block.Length - 1] == 'C')
                            {
                                string num = "" + block[block.Length - 9] +
                               block[block.Length - 8] + block[block.Length - 7] + block[block.Length - 6]
                               + block[block.Length - 5] + block[block.Length - 4] + block[block.Length -
                               3] + block[block.Length - 2];
                                int num1 = Convert.ToInt32(num, 16);
                                block = block.Substring(0, block.Length - num1);
                            }
                        }
                    }
                    else
                        return null;
                }
                byte[] res_r = new byte[block.Length / 4];
                string buf;
                for (int j = 0; j < block.Length / 4; j++)
                {
                    buf = "" + block[j * 4 + 1] + block[j * 4 + 2];
                    while (buf.Length % 2 != 0)
                        buf = "0" + buf;
                    res_r[j] = Convert.ToByte(buf, 16);
                }
                Array.Resize(ref res, res.Length + res_r.Length);
                Array.Copy(res_r, 0, res, res.Length - res_r.Length, res_r.Length);
            }
            return res;
        }
        // Функція шифрування RSA
        public byte[] EncryptRSA(byte[] data, BigInteger d_, BigInteger n_)
        {
            if (!RetBlocks(data, n_))
                return null;
            int len = n_.ToString("X").Length;
            BigInteger raund;
            string result = "", buf;
            for (int i = 0; i < blocks.Length; i++)
            {
                raund = powRSA(blocks[i], d_, n_);
                buf = raund.ToString("X");
                while (buf.Length < len)
                    buf = "0" + buf;
                result += buf;
            }
            if (result.Length % 2 == 0)
                result += "AA";
            else
                result += "B";
            byte[] res_byte = new byte[result.Length / 2];
            for (int i = 0; i < res_byte.Length; i++)
            {
                res_byte[i] = Convert.ToByte("" + result[i * 2] + result[i * 2 + 1],
               16);
            }
            return res_byte;
        }
        // Функція дешифрування RSA
        public byte[] DecryptRSA(byte[] data, BigInteger e_, BigInteger n_)
        {
            string bufer = "", b;
            string[] cont = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                cont[i] = ((int)data[i]).ToString("x2");
            }
            bufer = String.Join("", cont);
            if (bufer[bufer.Length - 1] == 'b')
                bufer = bufer.Substring(0, bufer.Length - 1);
            else
                if (bufer[bufer.Length - 2] == 'a' && bufer[bufer.Length - 1] == 'a')
                bufer = bufer.Substring(0, bufer.Length - 2);
            int size = n_.ToString("X").Length;
            blocks = new BigInteger[bufer.Length / size];
            for (int i = 0; i < bufer.Length / size; i++)
            {
                b = bufer.Substring(i * size, size);
                blocks[i] = BigInteger.Parse("0" + b,
               System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            BigInteger block;
            for (int i = 0; i < blocks.Length; i++)
            {
                block = powRSA(blocks[i], e_, n_);
                blocks[i] = block;
            }
            byte[] res = readBlocks(n_);
            if (res == null)
                return null;
            return res;
        }
    }
}
