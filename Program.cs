using Raylib_cs;

class Program {

    static void Main(string[] args)
    {   
        int CELL_WIDTH = 100;
        int CELL_HEIGHT = 100;
        int WIDTH = 1600;
        int HEIGHT = 800;
        int START_X = (WIDTH/2)-(CELL_WIDTH/2);

        bool insertMode = false;
        int selected = 0;
        int multiplier;
        bool alphaMode = false;

        Raylib.SetTraceLogLevel(TraceLogLevel.Fatal);
        Raylib.InitWindow(WIDTH, HEIGHT, "BrainFuck VisiEdit");

        int[] bfTape = new int[30000];
        List<string> bfSrc = [];

        while(!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            Raylib.SetTargetFPS(24);

            for (int i=0; i<bfTape.Length; i++) {
                int value = bfTape[i];

                int x = START_X+(10+CELL_HEIGHT)*i;
                int y = (HEIGHT/2)-(CELL_HEIGHT/2);
                if (START_X <= x && x <= WIDTH) {
                    Color drawColor = (i == selected) ? Color.Yellow:Color.White; 
                    Raylib.DrawRectangle(START_X+(10+CELL_HEIGHT)*i, y, CELL_WIDTH, CELL_HEIGHT, drawColor);

                    int valueWidth = Raylib.MeasureText($"{value}", 25);
                    Raylib.DrawText($"{value}", x+(CELL_WIDTH/2 - valueWidth/2), y+(CELL_HEIGHT/2 - 25/2), 25, Color.Black);

                    if (alphaMode) {
                        int asciiWidth = Raylib.MeasureText($"{(char)value}", 25);
                        Raylib.DrawText($"{(char)value}", x+(CELL_WIDTH/2 - asciiWidth/2), y+CELL_HEIGHT+5, 25, Color.Gray);
                    }
                }

            }

            if (Raylib.IsKeyReleased(KeyboardKey.I)) {
                insertMode = !insertMode;
                if (!insertMode) {
                    alphaMode = false;
                }
            }

            if (insertMode && Raylib.IsKeyReleased(KeyboardKey.A)) {
                alphaMode = !alphaMode;
            }

            if (insertMode) {
                if (Raylib.IsKeyPressed(KeyboardKey.Right)) {
                    if (selected + 1 <= 30000) {
                        selected++;
                        START_X -= CELL_WIDTH+10;
                        bfSrc.Add(">");
                    }
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Left)) {
                    if (selected - 1 >= 0) {
                        selected--;
                        START_X += CELL_WIDTH+10;
                        bfSrc.Add("<");
                    }
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Up)) {
                    if ((Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.LeftAlt)) 
                        ||(Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.RightAlt))
                        ||(Raylib.IsKeyDown(KeyboardKey.RightControl) && Raylib.IsKeyDown(KeyboardKey.RightAlt)) 
                        ||(Raylib.IsKeyDown(KeyboardKey.RightControl) && Raylib.IsKeyDown(KeyboardKey.LeftAlt))) {

                        multiplier = 15;

                    } else if (Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl)) {
                        multiplier = 5;
                    } else if (Raylib.IsKeyDown(KeyboardKey.LeftAlt) || Raylib.IsKeyDown(KeyboardKey.RightAlt)) {
                        multiplier = 10;
                    } else {
                        multiplier = 1;
                    }
                    Program.AddToSource(bfSrc, 1*multiplier);
                    Program.AddToTape(bfTape, selected, 1*multiplier);
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Down)) {
                    if ((Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.LeftAlt)) 
                        ||(Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.RightAlt))
                        ||(Raylib.IsKeyDown(KeyboardKey.RightControl) && Raylib.IsKeyDown(KeyboardKey.RightAlt)) 
                        ||(Raylib.IsKeyDown(KeyboardKey.RightControl) && Raylib.IsKeyDown(KeyboardKey.LeftAlt))) {

                        multiplier = 15;

                    } else if (Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl)) {
                        multiplier = 5;
                    } else if (Raylib.IsKeyDown(KeyboardKey.LeftAlt) || Raylib.IsKeyDown(KeyboardKey.RightAlt)) {
                        multiplier = 10;
                    } else {
                        multiplier = 1;
                    }
                    Program.SubToSource(bfSrc, 1*multiplier);
                    Program.AddToTape(bfTape, selected, -1*multiplier);
                }

                if (Raylib.IsKeyPressed(KeyboardKey.LeftBracket)) {

                }

                if (Raylib.IsKeyPressed(KeyboardKey.RightBracket)) {
                    
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Period)) {
                    bfSrc.Add(".");
                }
            }

            #region Display Controls
            Raylib.DrawText("I - to enter/exit insert mode", 20, 10, 25, Color.White);
            Raylib.DrawText("A - to enter/exit alpha mode", 20, 40, 25, Color.White);
            Raylib.DrawText("> - to move the tape to the right", 20, 70, 25, Color.White);
            Raylib.DrawText("< - to move the tape to the left", 20, 100, 25, Color.White);
            Raylib.DrawText(". - Show output of cell", 20, 130, 25, Color.White);
            Raylib.DrawText("Ctrl+S - Save to 'program.bf' in current dir", 20, 160, 25, Color.White);
            #endregion

            #region Display OtherTexts
            int width = Raylib.MeasureText("v1", 25);
            Raylib.DrawText("v1", 1600-width-10, 800-30, 25, Color.Green);
            
            width = Raylib.MeasureText("INSERT MODE", 35);
            if (insertMode) {
                Raylib.DrawText("INSERT MODE", 800-(width/2), 800-40, 35, Color.Green);
            }

            width = Raylib.MeasureText("ALPHA MODE", 35);
            if (alphaMode) {
                Raylib.DrawText("ALPHA MODE", 800-(width/2), 800-40-40, 35, Color.DarkBlue);
            }
            #endregion

            if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyReleased(KeyboardKey.S)) {
                File.WriteAllText("C:\\Dev\\BrainFK-VisiEdit\\program.bf", String.Join("", bfSrc));
                Console.WriteLine("here");
            }

            if (Raylib.IsKeyReleased(KeyboardKey.F1) && !insertMode) {
                AddToTape(bfTape, 0, 72);
                AddToSource(bfSrc, 72);
                AppendToSource(bfSrc, ".>");

                AddToTape(bfTape, 1, 101);
                AddToSource(bfSrc, 101);
                AppendToSource(bfSrc, ".>");

                AddToTape(bfTape, 2, 108);
                AddToSource(bfSrc, 108);
                AppendToSource(bfSrc, ".>");
            
                AddToTape(bfTape, 3, 108);
                AddToSource(bfSrc, 108);
                AppendToSource(bfSrc, ".>");

                AddToTape(bfTape, 4, 111);
                AddToSource(bfSrc, 111);
                AppendToSource(bfSrc, ".>");

                AddToTape(bfTape, 5, 32);
                AddToSource(bfSrc, 32);                
                AppendToSource(bfSrc, ".>");

                AddToTape(bfTape, 6, 87);
                AddToSource(bfSrc, 87);
                AppendToSource(bfSrc, ".>");

                AddToTape(bfTape, 7, 111);
                AddToSource(bfSrc, 111);
                AppendToSource(bfSrc, ".>");

                AddToTape(bfTape, 8, 114);
                AddToSource(bfSrc, 114);
                AppendToSource(bfSrc, ".>");

                AddToTape(bfTape, 9, 108);
                AddToSource(bfSrc, 108);
                AppendToSource(bfSrc, ".>");

                AddToTape(bfTape, 10, 100);
                AddToSource(bfSrc, 100);
                AppendToSource(bfSrc, ".>");
                selected = 11;
                START_X -= (CELL_WIDTH*11)+(10*11);
            }

            Raylib.EndDrawing();            
        }

        Raylib.CloseWindow();
        Console.WriteLine(String.Join("", bfSrc));

    }

    public static void AppendToSource(List<String> source, string key) {
        source.Add(key);
    }

    public static void AddToSource(List<String> source, int amount = 1) {
        for (int i=0; i<amount; i++) {
            source.Add("+");
        }
    }

    public static void SubToSource(List<String> source, int amount = 1) {
        for (int i=0; i<amount; i++) {
            source.Add("-");
        }
    }

    public static void AddToTape(int[] tape, int index, int amount = 1) {
        if (tape[index] + amount >= 255) {
            tape[index] = 255;
        } else {
            tape[index] += amount;
        }
    }

    public static int GetASCIICode(int keycode) {
        Console.WriteLine(keycode);
        return -1;
    }

}