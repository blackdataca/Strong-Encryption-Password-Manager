using System;

namespace MyIdMobile.Models
{
    [Serializable]
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}