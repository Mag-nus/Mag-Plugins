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
				damageList.AddColumn(typeof(HudStaticText), 35, null); // This cannot go any smaller without purning labels
				damageList.AddColumn(typeof(HudStaticText), 85, null);

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
			typelessMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Typeless].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Typeless].TotalDamage.ToString("#,##0");
			typelessMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Typeless].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Typeless].TotalDamage.ToString("#,##0");
			slashMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Slash].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Slash].TotalDamage.ToString("#,##0");
			slashMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Slash].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Slash].TotalDamage.ToString("#,##0");
			pierceMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Pierce].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Pierce].TotalDamage.ToString("#,##0");
			pierceMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Pierce].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Pierce].TotalDamage.ToString("#,##0");
			bludgeMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Bludge].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Bludge].TotalDamage.ToString("#,##0");
			bludgeMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Bludge].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Bludge].TotalDamage.ToString("#,##0");
			fireMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Fire].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Fire].TotalDamage.ToString("#,##0");
			fireMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Fire].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Fire].TotalDamage.ToString("#,##0");
			coldMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Cold].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Cold].TotalDamage.ToString("#,##0");
			coldMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Cold].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Cold].TotalDamage.ToString("#,##0");
			acidMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Acid].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Acid].TotalDamage.ToString("#,##0");
			acidMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Acid].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Acid].TotalDamage.ToString("#,##0");
			electricMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Electric].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle][DamageElement.Electric].TotalDamage.ToString("#,##0");
			electricMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Electric].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic][DamageElement.Electric].TotalDamage.ToString("#,##0");

			totalMeleeMissileText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].TotalDamage.ToString("#,##0");
			totalMagicText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].TotalDamage.ToString("#,##0");


			attacksText.Text = trackedCombat[AttackDirection.PlayerInitiated].TotalAttacks == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].TotalAttacks.ToString("#,##0") + " (" + Math.Round(trackedCombat[AttackDirection.PlayerInitiated].AttackSuccessPercent, 0).ToString("F0") + "%)";
			evadesText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].TotalAttacks == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].TotalAttacks.ToString("#,##0") + " (" + Math.Round(100 - trackedCombat[AttackDirection.PlayerReceived][AttackType.MeleeMissle].AttackSuccessPercent, 0).ToString("F0") + "%)";
			resistsText.Text = trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].TotalAttacks == 0 ? "" : trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].TotalAttacks.ToString("#,##0") + " (" + Math.Round(100 - trackedCombat[AttackDirection.PlayerReceived][AttackType.Magic].AttackSuccessPercent, 0).ToString("F0") + "%)";

			avgText.Text = trackedCombat[AttackDirection.PlayerInitiated].AverageNonCritAttack == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].AverageNonCritAttack.ToString("#,##0");
			maxText.Text = trackedCombat[AttackDirection.PlayerInitiated].MaxNonCritAttack == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].MaxNonCritAttack.ToString("#,##0");

			critsText.Text = trackedCombat[AttackDirection.PlayerInitiated].Crits == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].Crits.ToString("#,##0") + " (" + Math.Round(trackedCombat[AttackDirection.PlayerInitiated].CritPercent, 0).ToString("F0") + "%)";
			critsAvgText.Text = trackedCombat[AttackDirection.PlayerInitiated].AverageCritAttack == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].AverageCritAttack.ToString("#,##0");
			critsMaxText.Text = trackedCombat[AttackDirection.PlayerInitiated].MaxCritAttack == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].MaxCritAttack.ToString("#,##0");

			totalDmgText.Text = trackedCombat[AttackDirection.PlayerInitiated].TotalDamage == 0 ? "" : trackedCombat[AttackDirection.PlayerInitiated].TotalDamage.ToString("#,##0");
		}
	}
}
