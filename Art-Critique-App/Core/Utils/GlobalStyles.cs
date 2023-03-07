using Microsoft.Maui;

namespace Art_Critique.Core.Utils {
    class GlobalStyles {
        public static Style ButtonStyle() {
            var buttonStyle = new Style(typeof(Button)) {
                Setters = {
                new Setter { Property = VisualElement.WidthRequestProperty, Value = DeviceProperties.GetWidthPercent(20) },
                new Setter { Property = VisualElement.HeightRequestProperty, Value = DeviceProperties.GetHeightPercent(2) },
                new Setter { Property = Button.CornerRadiusProperty, Value = 25 },
                new Setter { Property = Button.FontSizeProperty, Value = 18 },
                new Setter { Property = Button.FontFamilyProperty, Value = "PragmaticaMedium" },
                new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold }
                }
            };
            return buttonStyle;
        }
        public static Style EntryStyle() {
            var entryStyle = new Style(typeof(Entry)) {
                Setters = {
                    new Setter { Property = VisualElement.WidthRequestProperty, Value = DeviceProperties.GetWidthPercent(20) },
                    new Setter { Property = Entry.PlaceholderColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.TextColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.HorizontalTextAlignmentProperty, Value = TextAlignment.Center },
                }
            };
            return entryStyle;
        }
    }
}
