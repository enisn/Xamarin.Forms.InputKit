# InputKit: AutoCompleteEntry

AutoCompleteEntry is a control that allows the user to type in a string and select an item from a dropdown list.

It's not supported on MAUI. (Xamarin Forms only)


<table>
<tr>
<td>
<a href="#"><img src="https://raw.githubusercontent.com/enisn/Xamarin.Forms.InputKit/develop/shreenshots/autocompleteentries_android.gif" width="270" height="480" alt="Xamarin Forms Slider Sticky Label" class="aligncenter size-medium" /></a>
</td>
<td>
<a href="#"><img src="https://raw.githubusercontent.com/enisn/Xamarin.Forms.InputKit/develop/shreenshots/autocompleteentries_ios.png" width="270" height="480" alt="Xamarin Forms Slider Sticky Label" class="aligncenter size-medium" /></a>
</td>
</tr>
</table>


### Properties
- **Placeholder**: (string) Placehodler Text
- **Title**: (string) Title will be shown top of this control
- **IconImage**: (string) Icons of this Entry. Icon will be shown left of this - control
- **Color**: (Color) Color of Icon Image. IconImage must be a PNG and have - Alpha channels. This fills all not-Alpha channels one color. Default is - Accent
- **ValidationMessage**: (string) This is message automaticly displayed when - this is not validated. **Use this one instead of annotationmessage**
- **AnnotationColor**: (Color) AnnotationMessage's color..
- **IsRequired**: (bool) IValidation implementation. Same with IsAnnotated
- **ItemsSource**: (IList) Suggestions items


_Work in progress..._