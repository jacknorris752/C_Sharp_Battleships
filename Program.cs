using System;
using System.Text.RegularExpressions;

namespace Battleships
{
    class Program
    {   
        //  Define Grid
        static int gridSize = 10;
        static Ship[,] gameGrid = new Ship[gridSize, gridSize];
        static int[,] shotsTakenGrid = new int[gridSize, gridSize];

        static int shotsTaken = 0;

        public static void Main(string[] args)
        {
            Ship[] ships = new Ship[3];
            ships[0] = new Destroyer();
            ships[1] = new Destroyer();
            ships[2] = new Battleship();

            //  Populate Grid
            foreach (Ship ship in ships)
            {
                while (true)
                {
                    if(TryToPlaceShip(gameGrid, ship)){ break; }
                }
            }

            Console.WriteLine("Welcome to Battleships...");
            Console.WriteLine("Cordinates are to be given as grid references, for example: C4");
            Console.WriteLine("Valid ranges are from A-J and 1-10");

            //  Start Game Loop
            while (ships[0].alive || ships[1].alive || ships[2].alive)
            {
                (int shotX, int shotY) = UserGuess();   //Determines if users input is valid or not and returns it
                Console.WriteLine("Firing...");
                CheckShot(shotX,shotY, ref gameGrid);   //Checks if shot has already been taken and handles shot
                shotsTaken++;
            }

            //  END OF GAME
            PrintGrid(gridSize,gameGrid);
            Console.WriteLine("You completed the game in {0} shots", shotsTaken);
        }

        //
        //  Checks the shot, prints a relevant message if a miss or already taken
        //  If a HIT then runs the .Hit() method on the given ship and marks the shot on the shotTakenGrid
        //
        public static void CheckShot(int x, int y, ref Ship[,] gameGrid)
        {
            if (shotsTakenGrid[x, y] != 1)
            {
                //shot not already taken
                shotsTakenGrid[x, y] = 1;
                if (gameGrid[x,y] != null)
                {
                    gameGrid[x, y].Hit();
                }
                else
                {
                    Console.WriteLine("MISS!");
                }
            }
            else
            {
                Console.WriteLine("Shot Already Taken");
            }
        }


        //
        //  Gets the user to input their guess, checks it is valid/not already taken and returns their chosen cordinates
        //
        public static (int,int) UserGuess()
        {
            Console.WriteLine("Where would you like to shoot?");
            String userInput = Console.ReadLine();
            (bool valid, int x, int y) = CheckUserInput(userInput);
            while (!valid)
            {
                //Console.WriteLine("Invalid inpiut, please try again...");
                userInput = Console.ReadLine();
                (valid, x, y) = CheckUserInput(userInput);
            }

            return (x,y);
        }


        //
        //  Breaks down the input into x and y. Returns true if valid, false if not
        //
        public static (bool, int, int) CheckUserInput(String input)
        {
            int userXInput, userYInput;
            String xInput = input.Substring(0, 1);
            String yInput = input.Substring(1);

            if (yInput.Length > 2)
            {
                Console.WriteLine("Input too long");
                return (false, 0, 0);
            }
            else if (yInput.Length > 0)
            {
                Regex rg = new Regex(@"[A-Ja-j]");
                MatchCollection matchedLetters = rg.Matches(xInput);
                if (matchedLetters.Count > 0)
                {
                    bool isNumeric = int.TryParse(yInput, out userYInput);
                    if (isNumeric)
                    {
                        if (userYInput > 0 && userYInput < 11)
                        {
                            //convert the letter to an x value
                            switch (xInput.ToLower())
                            {
                                case "a":
                                    userXInput = 0;
                                    break;
                                case "b":
                                    userXInput = 1;
                                    break;
                                case "c":
                                    userXInput = 2;
                                    break;
                                case "d":
                                    userXInput = 3;
                                    break;
                                case "e":
                                    userXInput = 4;
                                    break;
                                case "f":
                                    userXInput = 5;
                                    break;
                                case "g":
                                    userXInput = 6;
                                    break;
                                case "h":
                                    userXInput = 7;
                                    break;
                                case "i":
                                    userXInput = 8;
                                    break;
                                case "j":
                                    userXInput = 9;
                                    break;
                                default:
                                    userXInput = 0;
                                    break;
                            }
                            return (true, userXInput, userYInput - 1);
                        }
                        else Console.WriteLine("Invalid input: Y input out of range"); return (false, 0, 0);
                    }
                    else Console.WriteLine("Invalid input: Y Not Numeric"); return (false, 0, 0);
                }
                else Console.WriteLine("Invalid input: X Not a letter or in range"); return (false, 0, 0);
            }
            else Console.WriteLine("Invalid input: Input not long enough"); return (false, 0, 0);
        }


        //
        //  Return true is ship gets placed otherwise false if unable to place ship
        //
        public static bool TryToPlaceShip(Ship[,] gameGrid, Ship ship)
        {
            //Pick a Square 
            Random rand = new Random();
            int x = rand.Next(0, gridSize-1);
            int y = rand.Next(0, gridSize-1);
            CheckGridRef(gameGrid, x, y);

            ship.myOrientation = ship.ChooseOrientation();

            if (ship.myOrientation == Ship.orientation.Vertical)
            {
                #region place vertical ship
                //ship must be laid on x cordinates
                int shipLength = ship.size;
                bool shipLaid = false;
                while (!shipLaid)
                {
                    //go through the cordinates and check they're all free
                    for (int i = 0; i < shipLength; i++)
                    {
                        if(x+shipLength > gridSize - 1  || gameGrid[ (x+i), y] != null)
                        {
                            return false;
                        }
                    }
                    //given that all cordinates are free, place references to the given ship
                    for (int i = 0; i < shipLength; i++)
                    {
                        gameGrid[(x + i), y] = ship;
                    }
                    shipLaid = true;
                    return true;
                }
                #endregion
            }
            else
            {
                #region place horizontal ship
                //ship must be laid on y cordinates
                int shipLength = ship.size;
                bool shipLaid = false;
                while (!shipLaid)
                {
                    //go through the cordinates and check they're all free
                    for (int i = 0; i < shipLength; i++)
                    {
                        if (y+shipLength > gridSize - 1 || gameGrid[x, (y + i)] != null)
                        {
                            return false;
                        }
                    }
                    //given that all cordinates are free, place references to the given ship
                    for (int i = 0; i < shipLength; i++)
                    {
                        gameGrid[x, (y + i)] = ship;
                    }
                    shipLaid = true;
                    return true;
                }
                #endregion
            }
            return false;
        }


        //
        //  Checks if the given x y cordinates contain a ship or not
        //
        public static bool CheckGridRef(Ship[,] gameGrid, int x, int y)
        {
            if (gameGrid[x, y] == null)
            {
                return true;
            }
            return false;
        }


        //
        //  Prints out the ship grid
        //
        public static void PrintGrid(int gridSize, Ship[,] grid)
        {
            Console.WriteLine("");
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (y == gridSize-1) {
                        if (grid[x, y] == null)
                        {
                            Console.Write("o");
                        }
                        else
                        {
                            Console.Write("@");
                        }
                    }
                    else
                    {
                        if (grid[x, y] == null)
                        {
                            Console.Write("o.");
                        }
                        else
                        {
                            Console.Write("@.");
                        }
                    }       
                }
                Console.WriteLine("");
            }   
        }
    }
}
