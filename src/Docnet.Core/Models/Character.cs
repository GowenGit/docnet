namespace Docnet.Core.Models
{
    public struct Character
    {
        public char Char { get; }

        public BoundBox Box { get; }

        public Character(char character, BoundBox box)
        {
            Char = character;
            Box = box;
        }
 
        public override bool Equals(object obj)
        {
            if (!(obj is Character))
            {
                return false;
            }

            var character = (Character)obj;
            return Char == character.Char && Box.Equals(character.Box);
        }

        public override int GetHashCode()
        {
            var hashCode = 13;
            hashCode = hashCode * 7 + Char.GetHashCode();
            hashCode = hashCode * 7 + Box.GetHashCode();
            return hashCode;
        }
    }
}