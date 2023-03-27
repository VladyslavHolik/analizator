public class Program
{
    public static void Main()
    {
        var inputString = Console.ReadLine() ?? "";
        {
            var isHexNumber = Analyzer.GetValue(inputString, out var number);
            Console.WriteLine("Is hex floating point number: " + isHexNumber);
            Console.WriteLine("Result value: " + number);
        }
    }
}

internal static class Analyzer
{
    // Стани автомату
    private enum States
    {
        S,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K
    }

    private static readonly Dictionary<char, int> HexDigitValue = new()
    {
        { '0', 0 },
        { '1', 1 },
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
        { 'a', 10 },
        { 'b', 11 },
        { 'c', 12 },
        { 'd', 13 },
        { 'e', 14 },
        { 'f', 15 },
    };

    public static bool GetValue(string s, out double value)
    {
        value = 0; // Значення числа

        var sign = 1; // Знак числа
        var f = 0.0625; // Значення порядку дробної частини числа 1 / 16

        var position = 0; // Поточний номер позиції
        var state = States.S;

        var powerSign = 1; // знак степеню
        var powerValue = 0; // значення степеню

        while (state != States.F && state != States.E && s.Length > position)
        {
            var symbol = s[position]; // Символ, що аналізується
            switch (state)
            {
                case States.S:
                {
                    switch (symbol)
                    {
                        case '+':
                        {
                            state = States.A;
                            break;
                        }
                        case '-':
                        {
                            state = States.A;
                            sign = -1;
                            break;
                        }
                        case '0':
                        {
                            state = States.C;
                            break;
                        }
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case 'a':
                        case 'b':
                        case 'c':
                        case 'd':
                        case 'e':
                        case 'f':
                        {
                            state = States.B;
                            value = value * 16 + HexDigitValue[symbol];
                            break;
                        }
                        default:
                        {
                            // Помилка! Очікується знак або цифра.
                            state = States.E;
                            break;
                        }
                    }

                    break;
                }
                case States.A:
                {
                    switch (symbol)
                    {
                        case '0':
                        {
                            state = States.C;
                            break;
                        }
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case 'a':
                        case 'b':
                        case 'c':
                        case 'd':
                        case 'e':
                        case 'f':
                        {
                            state = States.B;
                            value = value * 16 + HexDigitValue[symbol];
                            break;
                        }
                        default:
                        {
                            // Помилка в цілій частині числа!
                            state = States.E;
                            break;
                        }
                    }

                    break;
                }
                case States.B:
                {
                    switch (symbol)
                    {
                        case ';':
                        {
                            state = States.F;
                            break;
                        }
                        case '.':
                        {
                            state = States.D;
                            break;
                        }
                        case 'p':
                        {
                            state = States.H;
                            break;
                        }
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case 'a':
                        case 'b':
                        case 'c':
                        case 'd':
                        case 'e':
                        case 'f':
                        {
                            state = States.B;
                            value = value * 16 + HexDigitValue[symbol];
                            break;
                        }
                        default:
                        {
                            // Помилка! Очікується ".", "p", ";" або цифра
                            state = States.E;
                            break;
                        }
                    }

                    break;
                }
                case States.C:
                {
                    switch (symbol)
                    {
                        case ';':
                        {
                            state = States.F;
                            break;
                        }
                        case '.':
                        {
                            state = States.D;
                            break;
                        }
                        default:
                        {
                            // Помилка! Очікується ";" або "."
                            state = States.E;
                            break;
                        }
                    }

                    break;
                }
                case States.D:
                {
                    switch (symbol)
                    {
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case 'a':
                        case 'b':
                        case 'c':
                        case 'd':
                        case 'e':
                        case 'f':
                        {
                            state = States.G;
                            value += f * HexDigitValue[symbol];
                            f /= 16;
                            break;
                        }
                        default:
                        {
                            // Помилка! Очікується цифра дробової частини
                            state = States.E;
                            break;
                        }
                    }

                    break;
                }
                case States.G:
                {
                    switch (symbol)
                    {
                        case ';':
                        {
                            state = States.F;
                            break;
                        }
                        case 'p':
                        {
                            state = States.H;
                            break;
                        }
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case 'a':
                        case 'b':
                        case 'c':
                        case 'd':
                        case 'e':
                        case 'f':
                        {
                            state = States.G;
                            value += f * HexDigitValue[symbol];
                            f /= 16;
                            break;
                        }
                        default:
                        {
                            // Помилка! Очікується "." або ";"
                            state = States.E;
                            break;
                        }
                    }

                    break;
                }
                case States.H:
                {
                    switch (symbol)
                    {
                        case '+':
                        {
                            state = States.I;
                            break;
                        }
                        case '-':
                        {
                            state = States.I;
                            powerSign = -1;
                            break;
                        }
                        case '0':
                        {
                            state = States.K;
                            break;
                        }
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        {
                            state = States.J;
                            powerValue = powerValue * 10 + HexDigitValue[symbol];
                            break;
                        }
                        default:
                        {
                            // Помилка! Очікується знак або цифра.
                            state = States.E;
                            break;
                        }
                    }

                    break;
                }
                case States.I:
                {
                    switch (symbol)
                    {
                        case '0':
                        {
                            state = States.K;
                            break;
                        }
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        {
                            state = States.J;
                            powerValue = powerValue * 10 + HexDigitValue[symbol];
                            f /= 10;
                            break;
                        }
                        default:
                        {
                            // Помилка!
                            state = States.E;
                            break;
                        }
                    }

                    break;
                }
                case States.J:
                {
                    switch (symbol)
                    {
                        case ';':
                        {
                            state = States.F;
                            break;
                        }
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        {
                            state = States.J;
                            powerValue = powerValue * 10 + HexDigitValue[symbol];
                            break;
                        }
                        default:
                        {
                            // Помилка!
                            state = States.E;
                            break;
                        }
                    }

                    break;
                }
                case States.K:
                {
                    state = symbol switch
                    {
                        ';' => States.F,
                        _ => States.E
                    };
                    break;
                }
            }

            position++;
        }

        value *= sign;
        powerValue *= powerSign;
        value *= Math.Pow(2, powerValue);
        return state == States.F;
    }
}
