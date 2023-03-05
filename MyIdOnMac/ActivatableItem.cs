﻿using System;
using AppKit;
using Foundation;

namespace MyIdOnMac
{
    [Register("ActivatableItem")]
    public class ActivatableItem : NSToolbarItem
    {
        public bool Active { get; set; } = true;

        public ActivatableItem()
        {
        }

        public ActivatableItem(IntPtr handle) : base(handle)
        {
        }

        public ActivatableItem(NSObjectFlag t) : base(t)
        {
        }

        public ActivatableItem(string title) : base(title)
        {
        }

        public override void Validate()
        {
            base.Validate();
            Enabled = Active;
        }
    }
}