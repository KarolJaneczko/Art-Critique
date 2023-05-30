using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique.Core.Services {
    public class StylesService : IStyles {
        #region Services
        private readonly IProperties Properties;
        #endregion

        #region Constructor
        public StylesService(IProperties properties) {
            Properties = properties;
        }
        #endregion

        #region Implementation of methods
        public Style ButtonStyle() {
            var buttonStyle = new Style(typeof(Button)) {
                Setters = {
                new Setter { Property = VisualElement.WidthRequestProperty, Value = Properties.GetWidthPercent(20) },
                new Setter { Property = VisualElement.HeightRequestProperty, Value = Properties.GetHeightPercent(2) },
                new Setter { Property = Button.CornerRadiusProperty, Value = 25 },
                new Setter { Property = Button.FontSizeProperty, Value = 18 },
                new Setter { Property = Button.FontFamilyProperty, Value = "PragmaticaMedium" },
                new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold }
                }
            };
            return buttonStyle;
        }

        public Style EntryStyle() {
            var entryStyle = new Style(typeof(Entry)) {
                Setters = {
                    new Setter { Property = VisualElement.WidthRequestProperty, Value = Properties.GetWidthPercent(20) },
                    new Setter { Property = Entry.PlaceholderColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.TextColorProperty, Value = Color.FromRgb(0, 0, 0) },
                    new Setter { Property = Entry.HorizontalTextAlignmentProperty, Value = TextAlignment.Center },
                }
            };
            return entryStyle;
        }
        #endregion
    }
}
