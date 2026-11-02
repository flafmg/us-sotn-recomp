using System;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;
using RecompOne.Runtime.Modding;

namespace Recompiled;

public static class ItemDescriptionLines
{
    private const uint CallReturn = 0x801B0F5C;
    private const int FirstLineY = 0x184;

     //editar esses valores pra config o tmnh, mas ach que vou ter que fazer outro patch para mudar o tamanho da fonte ou a fonte inteira na descrição, por enquanto wip
    private const int DefaultLineSpacing = 16;
    private const int LineSpacing = 16;

    [PreHook("main", "LoadTPage")]
    public static void AdjustLineY(CpuContext context, IMemory memory)
    {
        if (context.RA != CallReturn || context.A3 != 2) return;
        uint yAddress = context.SP + 0x10;
        uint widthAddress = context.SP + 0x14;
        uint heightAddress = context.SP + 0x18;
        int y = (short)memory.ReadU16(yAddress);
        if ((short)memory.ReadU16(widthAddress) != 0xFC || (short)memory.ReadU16(heightAddress) != 16 || y <= FirstLineY)
            return;

        int rowOffset = y - FirstLineY;
        if (rowOffset % DefaultLineSpacing != 0) return;
        int row = rowOffset / DefaultLineSpacing;
        memory.WriteU16(yAddress, (ushort)(FirstLineY + row * LineSpacing));
    }
}
