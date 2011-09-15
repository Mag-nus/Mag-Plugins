using System;

using VirindiViewService.Controls;

namespace MagTools.Trackers.Combat
{
	class CombatTrackerGUIInfo
	{
		HudList damageList;

		HudStaticText typelessMeleeMissileText = null;
		HudStaticText typelessMagicText = null;
		HudStaticText slashMeleeMissileText = null;
		HudStaticText slashMagicText = null;
		HudStaticText pierceMeleeMissileText = null;
		HudStaticText pierceMagicText = null;
		HudStaticText bludgeMeleeMissileText = null;
		HudStaticText bludgeMagicText = null;
		HudStaticText fireMeleeMissileText = null;
		HudStaticText fireMagicText = null;
		HudStaticText coldMeleeMissileText = null;
		HudStaticText coldMagicText = null;
		HudStaticText acidMeleeMissileText = null;
		HudStaticText acidMagicText = null;
		HudStaticText electricMeleeMissileText = null;
		HudStaticText electricMagicText = null;

		HudStaticText totalMeleeMissileText = null;
		HudStaticText totalMagicText = null;

		HudStaticText attacksText = null;
		HudStaticText evadesText = null;
		HudStaticText resistsText = null;

		HudStaticText avgText = null;
		HudStaticText maxText = null;

		HudStaticText critsText = null;
		HudStaticText critsAvgText = null;
		HudStaticText critsMaxText = null;

		HudStaticText totalDmgText = null;

		public CombatTrackerGUIInfo(HudList damageList)
		{
			try
			{
				this.damageList = damageList;

				damageList.ClearColumnsAndRows();

				damageList.AddColumn(typeof(HudStaticText), 45, null);
				damageList.AddColumn(typeof(HudStaticText), 45, null);
				damageList.AddColumn(typeof(HudStaticText), 45, null);
				damageList.AddColumn(typeof(HudStaticText), 10, null);
				damageList.AddColumn(typeof(HudStaticText), 50, null);
				damageList.AddColumn(typeof(HudStaticText), 65, null);

				HudList.HudListRowAccessor newRow;

				newRow = damageList.AddRow();
				((HudStaticText)newRow[1]).Text = "Mel/Msl";
				((HudStaticText)newRow[1]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[2]).Text = "Magic";
				((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Attacks";
				attacksText = ((HudStaticText)newRow[5]);
				attacksText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = damageList.AddRow();
				((HudStaticText)newRow[0]).Text = "Typeless";
				typelessMeleeMissileText = ((HudStaticText)newRow[1]);
				typelessMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				typelessMagicText = ((HudStaticText)newRow[2]);
				typelessMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Evades";
				evadesText = ((HudStaticText)newRow[5]);
				evadesText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = damageList.AddRow();
				((HudStaticText)newRow[0]).Text = "Slash";
				slashMeleeMissileText = ((HudStaticText)newRow[1]);
				slashMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				slashMagicText = ((HudStaticText)newRow[2]);
				slashMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Resists";
				resistsText = ((HudStaticText)newRow[5]);
				resistsText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = damageList.AddRow();
				((HudStaticText)newRow[0]).Text = "Pierce";
				pierceMeleeMissileText = ((HudStaticText)newRow[1]);
				pierceMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				pierceMagicText = ((HudStaticText)newRow[2]);
				pierceMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = damageList.AddRow();
				((HudStaticText)newRow[0]).Text = "Bludge";
				bludgeMeleeMissileText = ((HudStaticText)newRow[1]);
				bludgeMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				bludgeMagicText = ((HudStaticText)newRow[2]);
				bludgeMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Avg.";
				avgText = ((HudStaticText)newRow[5]);
				avgText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = damageList.AddRow();
				((HudStaticText)newRow[0]).Text = "Fire";
				fireMeleeMissileText = ((HudStaticText)newRow[1]);
				fireMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				fireMagicText = ((HudStaticText)newRow[2]);
				fireMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Max";
				maxText = ((HudStaticText)newRow[5]);
				maxText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = damageList.AddRow();
				((HudStaticText)newRow[0]).Text = "Cold";
				coldMeleeMissileText = ((HudStaticText)newRow[1]);
				coldMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				coldMagicText = ((HudStaticText)newRow[2]);
				coldMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Crits";
				critsText = ((HudStaticText)newRow[5]);
				critsText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = damageList.AddRow();
				((HudStaticText)newRow[0]).Text = "Acid";
				acidMeleeMissileText = ((HudStaticText)newRow[1]);
				acidMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				acidMagicText = ((HudStaticText)newRow[2]);
				acidMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Avg.";
				critsAvgText = ((HudStaticText)newRow[5]);
				critsAvgText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = damageList.AddRow();
				((HudStaticText)newRow[0]).Text = "Electric";
				electricMeleeMissileText = ((HudStaticText)newRow[1]);
				electricMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				electricMagicText = ((HudStaticText)newRow[2]);
				electricMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Max";
				critsMaxText = ((HudStaticText)newRow[5]);
				critsMaxText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = damageList.AddRow();

				newRow = damageList.AddRow();
				((HudStaticText)newRow[0]).Text = "Total";
				totalMeleeMissileText = ((HudStaticText)newRow[1]);
				totalMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				totalMagicText = ((HudStaticText)newRow[2]);
				totalMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[4]).Text = "Total";
				totalDmgText = ((HudStaticText)newRow[5]);
				totalDmgText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public void LoadFromTrackedCombat(ITrackedCombat trackedCombat)
		{
			typelessMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Typeless].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Typeless].TotalDamage.ToString();
			typelessMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Typeless].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Typeless].TotalDamage.ToString();
			slashMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Slash].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Slash].TotalDamage.ToString();
			slashMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Slash].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Slash].TotalDamage.ToString();
			pierceMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Pierce].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Pierce].TotalDamage.ToString();
			pierceMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Pierce].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Pierce].TotalDamage.ToString();
			bludgeMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Bludge].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Bludge].TotalDamage.ToString();
			bludgeMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Bludge].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Bludge].TotalDamage.ToString();
			fireMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Fire].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Fire].TotalDamage.ToString();
			fireMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Fire].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Fire].TotalDamage.ToString();
			coldMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Cold].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Cold].TotalDamage.ToString();
			coldMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Cold].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Cold].TotalDamage.ToString();
			acidMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Acid].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Acid].TotalDamage.ToString();
			acidMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Acid].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Acid].TotalDamage.ToString();
			electricMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Electric].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Electric].TotalDamage.ToString();
			electricMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Electric].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Electric].TotalDamage.ToString();

			totalMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].TotalDamage.ToString();
			totalMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].TotalDamage.ToString();


			attacksText.Text = trackedCombat[AttackDirection.PlayerInitiated].TotalAttacks == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].TotalAttacks.ToString() + " (" + trackedCombat[AttackDirection.PlayerInitiated].AttackSuccessPercent.ToString("F0") + "%)";
			evadesText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].TotalAttacks == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].TotalAttacks.ToString() + " (" + (100 - trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].AttackSuccessPercent).ToString("F0") + "%)";
			resistsText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].TotalAttacks == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].TotalAttacks.ToString() + " (" + (100 - trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].AttackSuccessPercent).ToString("F0") + "%)";

			avgText.Text = trackedCombat[AttackDirection.PlayerInitiated].AverageNonCritAttack == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].AverageNonCritAttack.ToString();
			maxText.Text = trackedCombat[AttackDirection.PlayerInitiated].MaxNonCritAttack == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].MaxNonCritAttack.ToString();

			critsText.Text = trackedCombat[AttackDirection.PlayerInitiated].Crits == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].Crits.ToString();
			critsAvgText.Text = trackedCombat[AttackDirection.PlayerInitiated].AverageCritAttack == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].AverageCritAttack.ToString();
			critsMaxText.Text = trackedCombat[AttackDirection.PlayerInitiated].MaxCritAttack == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].MaxCritAttack.ToString();

			totalDmgText.Text = trackedCombat[AttackDirection.PlayerInitiated].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].TotalDamage.ToString();
		}
	}
}
