﻿using AutoModPlugins.Properties;
using PKHeX.Core;
using PKHeX.Core.AutoMod;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AutoModPlugins
{
    public class LivingDex : AutoModPlugin
    {
        public override string Name => "Generate Living Dex";
        public override int Priority => 1;

        protected override void AddPluginControl(ToolStripDropDownItem modmenu)
        {
            var ctrl = new ToolStripMenuItem(Name) { Image = Resources.livingdex };
            ctrl.Click += GenLivingDex;
            ctrl.Name = "Menu_LivingDex";
            modmenu.DropDownItems.Add(ctrl);
        }

        private void GenLivingDex(object? sender, EventArgs e)
        {
            var prompt = WinFormsUtil.Prompt(MessageBoxButtons.YesNo, "Generate a Living Dex?");
            if (prompt != DialogResult.Yes)
                return;

            var sav = SaveFileEditor.SAV;
            Span<PKM> pkms = sav.GenerateLivingDex().ToArray();
            Span<PKM> bd = sav.BoxData.ToArray();
            var copied = pkms.TryCopyTo(bd);
            if (!copied)
            {
                pkms = pkms[..bd.Length];
                pkms.TryCopyTo(bd);
            }
            sav.BoxData = bd.ToArray();
            SaveFileEditor.ReloadSlots();

            System.Diagnostics.Debug.WriteLine($"Generated Living Dex with {pkms.Length} entries.");
        }
    }
}
