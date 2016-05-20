﻿using System;

namespace Tavarta.ViewModel.SlideShow
{
    public class SlideShowViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
    }
}