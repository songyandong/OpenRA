﻿using OpenRa.Game.Graphics;

namespace OpenRa.Game.Traits
{
	class RenderUnitRotorInfo : RenderUnitInfo
	{
		public override object Create(Actor self) { return new RenderUnitRotor(self); }
	}

	class RenderUnitRotor : RenderUnit
	{
		public Animation rotorAnim, secondRotorAnim;

		public RenderUnitRotor( Actor self )
			: base(self)
		{
			var unit = self.traits.Get<Unit>();

			rotorAnim = new Animation(self.LegacyInfo.Name);
			rotorAnim.PlayRepeating("rotor");
			anims.Add( "rotor_1", new AnimationWithOffset(
				rotorAnim,
				() => Util.GetTurretPosition( self, unit, self.LegacyInfo.RotorOffset, 0 ),
				null ) );

			if (self.LegacyInfo.RotorOffset2 == null) return;

			secondRotorAnim = new Animation( self.LegacyInfo.Name );
			secondRotorAnim.PlayRepeating( "rotor2" );
			anims.Add( "rotor_2", new AnimationWithOffset(
				secondRotorAnim,
				() => Util.GetTurretPosition(self, unit, self.LegacyInfo.RotorOffset2, 0),
				null ) );
		}

		public override void Tick(Actor self)
		{
			base.Tick(self);

			var unit = self.traits.Get<Unit>();
			
			var isFlying = unit.Altitude > 0;

			if (isFlying ^ (rotorAnim.CurrentSequence.Name != "rotor")) 
				return;

			rotorAnim.ReplaceAnim(isFlying ? "rotor" : "slow-rotor");
			if (secondRotorAnim != null)
				secondRotorAnim.ReplaceAnim(isFlying ? "rotor2" : "slow-rotor2");
		}
	}
}
