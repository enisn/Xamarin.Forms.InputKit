# InputKit: Dropdown
Alternative picker with dropdown menu.

It's not supported on MAUI. (Xamarin Forms only)

![InputKit maui dropdown xamarin forms](https://media.giphy.com/media/CjGR8p3HoeOup8r21J/giphy.gif)

### Properties

- **Placeholder**: (string) Placehodler Text
- **Title**: (string) Title will be shown top of this control
- **IconImage**: (string) Icons of this Entry. Icon will be shown left of this - control
- **Color**: (Color) Color of Icon Image. IconImage must be a PNG and have - Alpha channels. This fills all not-Alpha channels one color. Default is - Accent
- **ValidationMessage**: (string) This is message automaticly displayed when - this is not validated. **Use this one instead of annotationmessage**
- **AnnotationColor**: (Color) AnnotationMessage's color..
- **IsRequired**: (bool) IValidation implementation. Same with IsAnnotated

### Coloring

There are 6 properties for coloring dropdown and they are confusing. There is explanation for color properties below;

<a href="https://i.ibb.co/Zh0V2Kv/inputkit-dropdown.png" target="_blank"> 
   <img src="https://i.ibb.co/Zh0V2Kv/inputkit-dropdown.png" width="480" />
</a>

- **Color** : Simply defines, Icon and arrow color of dropdown. (_Icon can be set by `IconImage` property_)
- **TitleColor** : Defines title of this control. (_Title can be set by `Title` property_)
- **AnnotationColor** : Defines annotation color of this control. Annotation will be displayed when this field is required and unselected. This message can be set by 'ValidationMessage' property.
- **BorderColor** : Defined borders of dropdown. If this is set as `Color.Transparent`, shadow will be removed too.
- **TextColor** : Defines text color of dropdown when a value is selected.
- **PlaceholderColor** : Defines text color of dropdown when no value selected. Default is `WhiteSmoke`.

---

_Work in progress..._