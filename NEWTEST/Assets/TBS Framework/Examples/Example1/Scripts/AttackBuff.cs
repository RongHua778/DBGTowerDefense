using TbsFramework.Units;

namespace TbsFramework.Example1
{
    public class AttackBuff : Units.Buff
    {
        private int _factor;

        public AttackBuff(int duration, int factor)
        {
            Duration = duration;
            _factor = factor;
        }

        public int Duration { get; set; }
        public void Apply(Unit unit)
        {
            unit.AttackFactor += _factor;
        }

        public void Undo(Unit unit)
        {
            unit.AttackFactor -= _factor;
        }

        public Units.Buff Clone()
        {
            return new AttackBuff(Duration, _factor);
        }
    }
}