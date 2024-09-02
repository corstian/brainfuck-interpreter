using System.Numerics;
using System.Text;
using Xunit.Abstractions;

namespace BrainfuckInterpreter;

public class RandomTests
{
    private readonly ITestOutputHelper _output;

    public RandomTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    private readonly byte[] seed =
    [
        0, 160, 167, 0, 0, 0, 0, 0, 17, 96, 210, 9, 0, 160, 167, 0, 16, 161, 83, 147, 17, 192, 164, 19, 1, 112, 230,
        161, 23, 226, 250, 185, 32, 144, 128, 124, 99, 175, 153, 135, 241, 113, 136, 75, 244, 214, 129, 110, 48, 173,
        254, 108, 65, 10, 36, 197, 225, 37, 236, 98, 5, 71, 27, 250, 64, 56, 214, 203, 49, 79, 133, 10, 209, 75, 141,
        241, 42, 220, 165, 105, 80, 113, 71, 39, 85, 50, 69, 34, 193, 163, 47, 77, 77, 100, 85, 41, 96, 152, 202, 133,
        72, 132, 48, 185, 177, 237, 222, 214, 159, 88, 162, 95, 112, 237, 15, 151, 14, 31, 98, 113, 161, 233, 238, 217,
        74, 191, 207, 59, 128, 176, 255, 196, 3, 31, 27, 91, 145, 87, 251, 138, 184, 129, 150, 27, 144, 33, 186, 36, 97,
        241, 204, 40, 129, 247, 231, 38, 65, 70, 188, 136, 160, 128, 151, 71, 82, 21, 240, 41, 113, 137, 224, 49, 114,
        192, 168, 188, 176, 13, 40, 236, 33, 208, 195, 63, 97, 205, 88, 214, 172, 63, 161, 168, 192, 8, 52, 143, 129,
        136, 203, 183, 81, 131, 12, 100, 87, 8, 33, 84, 208, 177, 187, 220, 111, 0, 252, 81, 65, 107, 255, 238, 93, 184,
        127, 170, 224, 72, 247, 0, 194, 56, 123, 236, 49, 69, 125, 14, 62, 156, 47, 220, 240, 13, 87, 217, 211, 108, 71,
        245, 33, 209, 25, 188, 89, 110, 134, 56, 0, 65, 131, 5, 99, 72, 250, 11, 17, 207, 176, 82, 205, 126, 45, 185,
        16, 34, 92, 216, 24, 61, 91, 44, 1, 255, 101, 173, 132, 182, 180, 113, 32, 241, 249, 40, 199, 176, 252, 86, 241,
        32, 165, 102, 203, 76, 200, 65, 48, 238, 172, 3, 220, 160, 97, 65, 225, 244, 33, 55, 20, 91, 101, 216, 64, 89,
        253, 58, 16, 75, 18, 229, 209, 58, 216, 116, 51, 191, 15, 167, 80, 114, 171, 216, 212, 110, 196, 62, 193, 178,
        11, 178, 200, 240, 45, 134, 96, 121, 175, 110, 131, 206, 188, 142, 177, 28, 72, 124, 19, 147, 191, 203, 112,
        174, 57, 72, 213, 186, 129, 108, 161, 56, 97, 59, 239, 160, 211, 163, 128, 81, 178, 122, 165, 166, 199, 212,
        145, 198, 114, 48, 50, 21, 101, 242, 144, 162, 185, 214, 129, 4, 95, 100, 129, 134, 224, 148, 43, 230, 74, 184,
        160, 225, 39, 185, 14, 3, 68, 97, 113, 56, 86, 217, 124, 15, 36, 108, 176, 78, 13, 188, 193, 32, 115, 47, 97,
        156, 199, 4, 10, 58, 204, 131, 192, 41, 178, 71, 247, 2, 191, 189, 81, 114, 112, 51, 61, 86, 227, 101, 208, 178,
        150, 3, 231, 127, 194, 43, 65, 122, 212, 53, 89, 49, 252, 147, 224, 41, 115, 39, 123, 94, 155, 225, 49, 116,
        191, 79, 23, 179, 141, 95, 240, 206, 55, 172, 142, 242, 12, 233, 33, 32, 69, 23, 76, 5, 250, 83
    ];
    
    [Fact]
    public void RunRandomTest()
    {
        var interpreter = new Interpreter(Encoding.ASCII.GetString(seed));
        
        interpreter.Run();
    }
    
    [Fact]
    public void KeepRunningRandomTest()
    {
        var numeric = GetNumbers(seed);
        
        // Warning: the 6th iteration contains a failure condition. Program doesn't terminate?
        for (int i = 0; i < 5; i++)
        {
            numeric = ComputeLater(numeric);

            var interpreter = new Interpreter(Encoding
                .ASCII
                .GetString(
                    GetBytes(numeric)));

            interpreter.Run();

            _output.WriteLine($"{interpreter.OperationCount}\t{interpreter.ProgramPointer}");
        }
    }

    private ulong ConstantAt(int i)
    {
        BigInteger Odd(int x) => 512 * BigInteger.Pow(15, x);
        BigInteger Even(int x) => Odd(x) / 15 * x;
        
        return (ulong)((Odd(i) + (Even(i) * BigInteger.Pow(2, 32))) % BigInteger.Pow(2, 64));
    }

    private ulong[] GetNumbers(byte[] bytes)
    {
        var results = new ulong[bytes.Length / 8];
        
        for (var i = 0; i < bytes.Length / 8; i++)
        {
            results[i] = BitConverter.ToUInt64(bytes, i * 8);
        }
        
        return results;
    }

    private ulong[] ComputeLater(ulong[] numbers)
    {
        for (var i = 0; i < numbers.Length; i++)
        {
            numbers[i] += ConstantAt(i);
        }

        return numbers;
    }

    private byte[] GetBytes(ulong[] numbers)
    {
        var buffer = new byte[512];

        for (var i = 0; i < numbers.Length; i++)
        {
            Array.Copy(BitConverter.GetBytes(numbers[i]), 0, buffer, i * 8, 8);
        }

        return buffer;
    }
}
