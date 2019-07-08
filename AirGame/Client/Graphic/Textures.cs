using static GlLib.Client.Graphic.Vertexer;

namespace GlLib.Client.Graphic
{
    public static class Textures
    {
        private const string SimpleStructPath = @"simple_structs/";
        private const string SystemPath = @"system/";

        public static Texture itemClasses;
        public static Texture slotSelected;
        public static Texture slotSwitch;
        public static Texture windowBack;
        public static Texture airBlow;
        public static Texture airShield;
        public static Texture bat;
        public static Texture bonePile;
        public static Texture box;
        public static Texture castle;
        public static Texture coin;
        public static Texture fireBall;
        public static Texture healthPotion;
        public static Texture slimeIdle;
        public static Texture slimeWalk;
        public static Texture streetLight;
        public static Texture spawn;
        public static Texture equipBackground;
        public static Texture scrollBar;
        public static Texture scroller;
        public static Texture monochromatic;
        public static Texture alagard;
        public static Texture esogam;
        public static Texture button;
        public static Texture buttonSelected;
        public static Texture buttonDisabled;
        public static Texture barStart;
        public static Texture barCenter;
        public static Texture barEnd;
        public static Texture barFiller;
        public static Texture slot;
        public static Texture background;
        public static Texture goldApple;
        public static Texture goldRubyRing;
        public static Texture commonSword;
        public static Texture dawnChestplate;
        public static Texture dawnBoots;
        public static Texture dawnShield;
        public static Texture dawnBlade;
        public static Texture varia;
        public static Texture slimeDeath;

        static Textures()
        {
            itemClasses = LoadTexture("gui/item_classes.png");
            slotSelected = LoadTexture("gui/slot_selected.png");
            slotSwitch = LoadTexture("gui/slot_switch.png");
            windowBack = LoadTexture("gui/window_back.png");
            airBlow = LoadTexture("air_blow.png");
            airShield = LoadTexture("air_shield.png");
            bat = LoadTexture("bat.png");
            bonePile = LoadTexture(SimpleStructPath + "Relics.png");
            box = LoadTexture(SimpleStructPath + "Box.png");
            castle = LoadTexture(SimpleStructPath + "Castle.png");
            coin = LoadTexture(SimpleStructPath + "Coins.png");
            fireBall = LoadTexture("11_fire_spritesheet.png");
            healthPotion = LoadTexture(SimpleStructPath + "HealthPotion.png");
            slimeIdle = LoadTexture("slime/slime_idle.png");
            slimeDeath = LoadTexture("slime/slime_death.png");
            slimeWalk = LoadTexture("slime/slime_jump.png");
            streetLight = LoadTexture(SimpleStructPath + "Streetlight.png");
            spawn = LoadTexture(SystemPath + "spawn.png");
            equipBackground = LoadTexture("gui/equipment_sub.png");
            scrollBar = LoadTexture("gui/scroll_bar.png");
            scroller = LoadTexture("gui/scroller.png");
            monochromatic = LoadTexture("monochromatic.png");
            alagard = LoadTexture("fonts/alagard.png");
            esogam = LoadTexture("fonts/esogam.png");
            button = LoadTexture("gui/button.png");
            buttonSelected = LoadTexture("gui/button_selected.png");
            buttonDisabled = LoadTexture("gui/button_disabled.png");
            barStart = LoadTexture("gui/bar_start.png");
            barCenter = LoadTexture("gui/bar_center.png");
            barEnd = LoadTexture("gui/bar_end.png");
            barFiller = LoadTexture("gui/bar_filler.png");
            slot = LoadTexture("gui/slot.png");
            background = LoadTexture("background.png");
            goldApple = LoadTexture("items/gold_apple.png");
            goldRubyRing = LoadTexture("items/gold_ruby_ring.png");
            commonSword = LoadTexture("items/common_sword.png");
            dawnChestplate = LoadTexture("items/dawn_chestplate.png");
            dawnBoots = LoadTexture("items/dawn_boots.png");
            dawnShield = LoadTexture("items/dawn_shield.png");
            dawnBlade = LoadTexture("items/dawn_blade.png");
            varia = LoadTexture("items/varia.png");
        }
    }
}