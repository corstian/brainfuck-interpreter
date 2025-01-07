using System.Diagnostics;
using System.Text;
using Xunit.Abstractions;

namespace BrainfuckInterpreter.Tests;

public class RandomTests : LfsrBase
{
    private readonly ITestOutputHelper _output;

    public RandomTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void RunRandomTest()
    {
        var interpreter = new Interpreter(Encoding.ASCII.GetString(Seed));
        
        interpreter.Run();
    }

    [Fact]
    public void SeedIndex()
    {
        var numeric = GetNumbers(Seed);
        
        Assert.Equal((ulong)10985472, numeric[0]);
    }

    [Fact]
    public void Previous()
    {
        var numeric = GetNumbers(Seed);
        Assert.Equal((ulong)10984960, ComputePrevious(numeric)[0]);
    }

    [Fact]
    public void GoesToZero()
    {
        var numeric = GetNumbers(Seed);

        while (numeric[0] != 0) numeric = ComputePrevious(numeric);
        
        Assert.Equal((ulong)0, numeric[0]);
    }
    
    public bool ContainsSubsequence<T>(List<T> sequence, List<T> subsequence)
    {
        return
            Enumerable
                .Range(0, sequence.Count - subsequence.Count + 1)
                .Any(n => sequence.Skip(n).Take(subsequence.Count).SequenceEqual(subsequence));
    }

    /*
       0000:7d6d 42              ??         42h    B
       0000:7d6e 4f              ??         4Fh    O
       0000:7d6f 4f              ??         4Fh    O
       0000:7d70 54              ??         54h    T
       0000:7d71 4d              ??         4Dh    M
       0000:7d72 47              ??         47h    G
       0000:7d73 52              ??         52h    R
       0000:7d74 20              ??         20h     
       0000:7d75 20              ??         20h     
       0000:7d76 20              ??         20h     
       0000:7d77 20              ??         20h     

     */
    [Fact]
    public void GoesBeyondZero()
    {
        var numeric = GetNumbers(Seed);

        List<byte> seq =
            //[0x42, 0x4f, 0x4f, 0x54,
            [0x4d, 0x47, 0x52];
            //, 0x20, 0x20, 0x20, 0x20];
        
        while (numeric[0] != 0)
        {
            numeric = ComputePrevious(numeric);

            if (ContainsSubsequence(GetBytes(numeric).ToList(), seq))
            {
                Debugger.Break();
            }
        }

        for (var i = 0; i <= 64; i++)
        {
            numeric = ComputePrevious(numeric);

            if (ContainsSubsequence(GetBytes(numeric).ToList(), seq))
            {
                Debugger.Break();
            } 
        }
    }
    
    [Fact]
    public void KeepRunningRandomTest()
    {
        var numeric = GetNumbers(Seed);
        
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
}
