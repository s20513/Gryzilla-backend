﻿using System;
using System.Collections.Generic;

namespace Gryzilla_App
{
    public partial class AchievementUser
    {
        public int IdUser { get; set; }
        public int IdAchievement { get; set; }
        public DateTime ReceivedAt { get; set; }

        public virtual Achievement IdAchievementNavigation { get; set; } = null!;
        public virtual UserDatum IdUserNavigation { get; set; } = null!;
    }
}