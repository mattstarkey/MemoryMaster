using System;

public static class StringMethods
{
    private static readonly char delim = ',';
    private static readonly byte maxColumns = 6;

    public static int MISSED = 0;
    public static int PROCESSED = 0;

    private static int airportId = 0;
    private static int planeNumber = 0;
    private static int planeId = 0;
    private static int flightMiles = 0;
    private static decimal fuelCost = 0;
    private static Dictionary<int, byte> charReference = new Dictionary<int, byte>()
        {
            {48, 0},{49, 1},{50, 2},{51, 3},{52, 4},{53, 5},{54, 6},{55, 7},{56, 8},{57, 9},{0, 0}
        };

    public static LineHolderStruct ToLineHolderStruct(this string str)
    {
        char[] chunk = new char[6];
        byte count = 0;
        byte index = 0;
        for (int i = 0; i < str.Length; i++)
        {
            char k = str[i];

            if (Char.IsDigit(k) || k == 46)
            {
                // Build up a chunk of characters to process.
                chunk[index] = k;
                // We need this index to keep track of the length of characters in the char array as we can't rely on it's length.
                index++;
            }
            else
            {
                if (k == delim)
                {
                    // if character is a delimeter, process the chunk and move to the next one.
                    switch (count)
                    {
                        case 1:
                            airportId = chunk.CharsToInt(index);
                            break;
                        case 2:
                            planeNumber = chunk.CharsToInt(index);
                            break;
                        case 3:
                            planeId = chunk.CharsToInt(index); ;
                            break;
                        case 4:
                            flightMiles = chunk.CharsToInt(index);
                            break;
                        case 5:
                            //fuelCost = Decimal.Parse(new string(chunk));
                            fuelCost = chunk.CharsToDecimal(index);
                            break;
                    }

                    // Clear the chunk and process the next one
                    chunk = chunk.ClearAll();
                    count++;
                    index = 0;
                }
            }

            if (count == maxColumns)
            {
                break;
            }
        }

        PROCESSED++;
        return new LineHolder(airportId, planeNumber, planeId, flightMiles, fuelCost);
    }

    public static LineHolder ToLineHolder(this string str)
    {
        char[] chunk = new char[6];
        byte count = 0;
        byte index = 0;
        for (int i = 0; i < str.Length; i++)
        {
            char k = str[i];

            if (i == 0 && k != 77)
            {
                return null;
            }

            if (Char.IsDigit(k) || k == 46)
            {
                // Build up a chunk of characters to process.
                chunk[index] = k;
                // We need this index to keep track of the length of characters in the char array as we can't rely on it's length.
                index++;
            }
            else
            {
                if (k == delim)
                {
                    // if character is a delimeter, process the chunk and move to the next one.
                    switch (count)
                    {
                        case 1:
                            airportId = chunk.CharsToInt(index);
                            break;
                        case 2:
                            planeNumber = chunk.CharsToInt(index);
                            break;
                        case 3:
                            planeId = chunk.CharsToInt(index); ;
                            break;
                        case 4:
                            flightMiles = chunk.CharsToInt(index);
                            break;
                        case 5:
                            //fuelCost = Decimal.Parse(new string(chunk));
                            fuelCost = chunk.CharsToDecimal(index);
                            break;
                    }

                    // Clear the chunk and process the next one
                    chunk = chunk.ClearAll();
                    count++;
                    index = 0;
                }
            }

            if (count == maxColumns)
            {
                break;
            }
        }

        PROCESSED++;
        return new LineHolder(airportId, planeNumber, planeId, flightMiles, fuelCost);
    }

    private static bool IsMNO(this string str, int i, char k)
    {
        if (i == 0 && k == 77)
        {
            if (str[i + 1] == 78)
            {
                if (str[i + 2] == 79)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static int CharsToInt(this char[] chars, int index)
    {
        int result = 0;
        int multiplier = 1;

        for (int i = index; i-- > 0;)
        {
            char c = chars[i];
            byte x = charReference[c];
            result += x * multiplier;
            multiplier = multiplier * 10;
        }

        return result;
    }

    public static decimal CharsToDecimal(this char[] chars, int index)
    {
        decimal result = 0;
        int multiplier = 1;
        int divider = 0;

        for (int i = index; i-- > 0;)
        {
            char c = chars[i];
            if (c == 46)
            {
                divider = multiplier;
            }
            else
            {
                byte x = charReference[c];
                result += x * multiplier;
                multiplier = multiplier * 10;
            }
        }

        if (divider > 0)
        {
            result /= divider;
        }

        return result;
    }

    public static char[] ClearAll(this char[] chars)
    {
        for (int j = 0; j < chars.Length; j++)
        {
            chars[j] = '0';
        }

        return chars;
    }

    public static string FilterBy(this string str)
    {
        if (str[0] == 'M' && str[1] == 'N' && str[2] == 'O')
        {
            return str;
        }
        return String.Empty;
    }
}
