using System;

using Mag.Shared;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class CombatTrackerGUIInfo
	{
		readonly HudStaticText typelessMeleeMissileText;
		readonly HudStaticText typelessMagicText;
		readonly HudStaticText slashMeleeMissileText;
		readonly HudStaticText slashMagicText;
		readonly HudStaticText pierceMeleeMissileText;
		readonly HudStaticText pierceMagicText;
		readonly HudStaticText bludgeMeleeMissileText;
		readonly HudStaticText bludgeMagicText;
		readonly HudStaticText fireMeleeMissileText;
		readonly HudStaticText fireMagicText;
		readonly HudStaticText coldMeleeMissileText;
		readonly HudStaticText coldMagicText;
		readonly HudStaticText acidMeleeMissileText;
		readonly HudStaticText acidMagicText;
		readonly HudStaticText electricMeleeMissileText;
		readonly HudStaticText electricMagicText;

		readonly HudStaticText totalMeleeMissileText;
		readonly HudStaticText totalMagicText;

		readonly HudStaticText attacksText;
		readonly HudStaticText evadesText;
		readonly HudStaticText resistsText;
		readonly HudStaticText aSurgesText;
		readonly HudStaticText cSurgesText;

		readonly HudStaticText avgMaxText;
		readonly HudStaticText critsText;
		readonly HudStaticText critsAvgMaxText;

		readonly HudStaticText totalDmgText;

		public CombatTrackerGUIInfo(HudList hudList)
		{
			try
			{
				hudList.ClearColumnsAndRows();

				// Each character is a max of 6 pixels wide
				hudList.AddColumn(typeof(HudStaticText), 40, null);
				hudList.AddColumn(typeof(HudStaticText), 52, null);
				hudList.AddColumn(typeof(HudStaticText), 52, null);
				hudList.AddColumn(typeof(HudStaticText), 45, null); // This cannot go any smaller without purning labels
				hudList.AddColumn(typeof(HudStaticText), 96, null);

				HudList.HudListRowAccessor newRow;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[1]).Text = "Mel/Msl";
				((HudStaticText)newRow[1]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[2]).Text = "Magic";
				((HudStaticText)newRow[2]).TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Attacks";
				attacksText = ((HudStaticText)newRow[4]);
				attacksText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Typeless";
				typelessMeleeMissileText = ((HudStaticText)newRow[1]);
				typelessMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				typelessMagicText = ((HudStaticText)newRow[2]);
				typelessMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Evades";
				evadesText = ((HudStaticText)newRow[4]);
				evadesText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Slash";
				slashMeleeMissileText = ((HudStaticText)newRow[1]);
				slashMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				slashMagicText = ((HudStaticText)newRow[2]);
				slashMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Resists";
				resistsText = ((HudStaticText)newRow[4]);
				resistsText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Pierce";
				pierceMeleeMissileText = ((HudStaticText)newRow[1]);
				pierceMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				pierceMagicText = ((HudStaticText)newRow[2]);
				pierceMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "A.Surges";
				aSurgesText = ((HudStaticText)newRow[4]);
				aSurgesText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Bludge";
				bludgeMeleeMissileText = ((HudStaticText)newRow[1]);
				bludgeMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				bludgeMagicText = ((HudStaticText)newRow[2]);
				bludgeMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "C.Surges";
				cSurgesText = ((HudStaticText)newRow[4]);
				cSurgesText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Fire";
				fireMeleeMissileText = ((HudStaticText)newRow[1]);
				fireMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				fireMagicText = ((HudStaticText)newRow[2]);
				fireMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Cold";
				coldMeleeMissileText = ((HudStaticText)newRow[1]);
				coldMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				coldMagicText = ((HudStaticText)newRow[2]);
				coldMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Av/Mx";
				avgMaxText = ((HudStaticText)newRow[4]);
				avgMaxText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Acid";
				acidMeleeMissileText = ((HudStaticText)newRow[1]);
				acidMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				acidMagicText = ((HudStaticText)newRow[2]);
				acidMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Crits";
				critsText = ((HudStaticText)newRow[4]);
				critsText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Electric";
				electricMeleeMissileText = ((HudStaticText)newRow[1]);
				electricMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				electricMagicText = ((HudStaticText)newRow[2]);
				electricMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Av/Mx";
				critsAvgMaxText = ((HudStaticText)newRow[4]);
				critsAvgMaxText.TextAlignment = VirindiViewService.WriteTextFormats.Right;

				hudList.AddRow();

				newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Total";
				totalMeleeMissileText = ((HudStaticText)newRow[1]);
				totalMeleeMissileText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				totalMagicText = ((HudStaticText)newRow[2]);
				totalMagicText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
				((HudStaticText)newRow[3]).Text = "Total";
				totalDmgText = ((HudStaticText)newRow[4]);
				totalDmgText.TextAlignment = VirindiViewService.WriteTextFormats.Right;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}


		public void Clear()
		{
			TypelessMeleeMissile = 0;
			TypelessMagic = 0;
			SlashMeleeMissile = 0;
			SlashMagic = 0;
			PierceMeleeMissile = 0;
			PierceMagic = 0;
			BludgeMeleeMissile = 0;
			BludgeMagic = 0;
			FireMeleeMissile = 0;
			FireMagic = 0;
			ColdMeleeMissile = 0;
			ColdMagic = 0;
			AcidMeleeMissile = 0;
			AcidMagic = 0;
			ElectricMeleeMissile = 0;
			ElectricMagic = 0;

			TotalMeleeMissile = 0;
			TotalMagic = 0;

			SetAttacks(0, 0);
			SetEvades(0, 0);
			SetResists(0, 0);

			SetAetheriaSurges(0, 0);
			SetCloakSurges(0, 0);

			SetAvgMax(0, 0);
			SetCrits(0, 0);
			SetCritsAvgMax(0, 0);

			TotalDamage = 0;
		}


		public int TypelessMeleeMissile
		{
			set { typelessMeleeMissileText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int TypelessMagic
		{
			set { typelessMagicText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int SlashMeleeMissile
		{
			set { slashMeleeMissileText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int SlashMagic
		{
			set { slashMagicText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int PierceMeleeMissile
		{
			set { pierceMeleeMissileText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int PierceMagic
		{
			set { pierceMagicText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int BludgeMeleeMissile
		{
			set { bludgeMeleeMissileText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int BludgeMagic
		{
			set { bludgeMagicText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int FireMeleeMissile
		{
			set { fireMeleeMissileText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int FireMagic
		{
			set { fireMagicText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int ColdMeleeMissile
		{
			set { coldMeleeMissileText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int ColdMagic
		{
			set { coldMagicText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int AcidMeleeMissile
		{
			set { acidMeleeMissileText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int AcidMagic
		{
			set { acidMagicText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int ElectricMeleeMissile
		{
			set { electricMeleeMissileText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int ElectricMagic
		{
			set { electricMagicText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}


		public int TotalMeleeMissile
		{
			set { totalMeleeMissileText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}

		public int TotalMagic
		{
			set { totalMagicText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}


		public void SetAttacks(int total, float percent)
		{
			attacksText.Text = total == 0 ? "" : Util.NumberFormatter(total, ("#,##0"), 999999) + " (" + Math.Round(percent, 0).ToString("F0") + "%)";
		}

		public void SetEvades(int total, float percent)
		{
			evadesText.Text = total == 0 ? "" : Util.NumberFormatter(total, ("#,##0"), 999999) + " (" + Math.Round(percent, 0).ToString("F0") + "%)";
		}

		public void SetResists(int total, float percent)
		{
			resistsText.Text = total == 0 ? "" : Util.NumberFormatter(total, ("#,##0"), 999999) + " (" + Math.Round(percent, 0).ToString("F0") + "%)";
		}

		public void SetAetheriaSurges(int total, float percent)
		{
			aSurgesText.Text = total == 0 ? "" : Util.NumberFormatter(total, ("#,##0"), 999999) + " (" + Math.Round(percent, 1).ToString("F1") + "%)";
		}

		public void SetCloakSurges(int total, float percent)
		{
			cSurgesText.Text = total == 0 ? "" : total.ToString("#,##0") + " (" + Math.Round(percent, 1).ToString("F1") + "%)";
		}


		public void SetAvgMax(int avg, int max)
		{
			avgMaxText.Text = avg == 0 ? "" : avg.ToString("#,##0") + " / " + max.ToString("#,##0");
		}

		public void SetCrits(int total, float percent)
		{
			critsText.Text = total == 0 ? "" : Util.NumberFormatter(total, ("#,##0"), 999999) + " (" + Math.Round(percent, 1).ToString("F1") + "%)";
		}

		public void SetCritsAvgMax(int avg, int max)
		{
			critsAvgMaxText.Text = avg == 0 ? "" : avg.ToString("#,##0") + " / " + max.ToString("#,##0");
		}


		public long TotalDamage
		{
			set { totalDmgText.Text = value == 0 ? "" : value.ToString("#,##0"); }
		}
	}
}
