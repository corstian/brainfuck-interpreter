using Newtonsoft.Json;
using r2pipe;

namespace BrainfuckInterpreter.Tests;

public class R2Tests : LfsrBase
{
    [Fact]
    public async Task Test()
    {
        Assert.Equal(12, await GetNumberOfInvalidOpcodes(Seed));
    }

    private async Task<int> GetNumberOfInvalidOpcodes(byte[] data)
    {
        using var tmpFile = new TemporaryFile();
        await File.WriteAllBytesAsync(tmpFile.FilePath, data);
        
        using IR2Pipe pipe = new R2Pipe(tmpFile.FilePath);

        await pipe.RunCommandAsync("e asm.arch=x86");
        await pipe.RunCommandAsync("e asm.bits=32");
        
        var res = await pipe.RunCommandAsync("pdj");
        var disasm = JsonConvert.DeserializeObject<IEnumerable<Disasm>>(res);

        return disasm.Count(q => q.opcode == "invalid");
    }

    [Fact]
    public async Task CollectInvalidOpcodes()
    {
        // INITIALIZE
        
        var numeric = GetNumbers(Seed);

        while (numeric[0] != 0) numeric = ComputePrevious(numeric);

        // PREPARE OUTPUT

        // await using var file = File.Open(, FileMode.Create);
        using var writer = new StreamWriter("./lfsr-invalid-opcodes.csv");

        writer.AutoFlush = true;
        // ITERATE

        int i = 0;
        while (i < 1.6e9)
        {
            var invalidOpcodes = await GetNumberOfInvalidOpcodes(GetBytes(numeric));
            
            await writer.WriteLineAsync($"{i},{invalidOpcodes}");
            
            numeric = ComputeLater(numeric);
            i += 512;
        }

        await writer.FlushAsync();
    }
}