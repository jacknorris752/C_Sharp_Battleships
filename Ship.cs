using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    abstract class Ship
    {

        public String name;
        public int health;
        public int size;
        public bool alive = true;
        public enum orientation {Horizontal,Vertical};
        public orientation myOrientation;

        public void Destroyed()
        {
            this.alive = false;
            Console.WriteLine("You have sunk a " + this.name);
        }

        public void Hit()
        {
            if(this.health == 1)
            {
                this.Destroyed();
            }
            else
            {
                Console.WriteLine("HIT!");
                this.health--;
            }
        }

        public orientation ChooseOrientation()
        {
            var rand = new Random();
            int randomNumber = rand.Next(0,2);

            switch (randomNumber)
            {
                case 0:
                    return orientation.Horizontal;
                case 1:
                    return  orientation.Vertical;
                default:
                    return ChooseOrientation();
                    //return orientation.Vertical;
            }
        }

    }

    class Destroyer : Ship
    {
        public Destroyer()
        {
            name = "Destroyer";
            health = 4;
            size = 4;
        }
    }

    class Battleship : Ship
    {
        public Battleship()
        {
            name = "Battleship";
            health = 5;
            size = 5;
        }
    }

}
