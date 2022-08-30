# InputKit: AdvancedEntry
AdvancedEntry is an upgraded version of regular entry. It includes a title, icon (colorable), validation and validation messages focusing next entry when done and more. It speeds up the development process. It's fully customizable and fully bindable.


![InputKit advanced entry](https://camo.githubusercontent.com/e0de2b39906d37de4614fb5fd7e369d33bfb21e349d878a9daf52baee12ef827/68747470733a2f2f6d656469612e67697068792e636f6d2f6d656469612f317a6c3075374f32646f4e6f6c49586e72542f67697068792e676966)

### Properties

- **Text**: (string) Text of user typed
- **Title**: (string) Title will be shown top of this control
- **IconImage**: (string) Icons of this Entry. Icon will be shown left of this control
- **IconColor**: (Color) Color of Icon Image. IconImage must be a PNG and have Alpha channels. This fills all - not-Alpha channels one color. Default is Accent
- **Placeholder**: (string) Entry's placeholder.
- **MaxLength**: (int) Text's Maximum length can user type.
- **MinLength**: (int) Text's Minimum length to be validated.
- **AnnotationMessage**: (string) This will be shown below title. This automaticly updating. If you set this - manually you must set true IgnoreValidationMessage !!! .
- **AnnotationColor**: (Color) AnnotationMessage's color..
- **Annotation**: (Enum) There is some annotation types inside in kit.
- **IsDisabled**: (bool) Sets this control disabled or not.
- **IsAnnotated**: (bool) Gets this control annotated or not. Depends on Annotation
- **IsRequired**: (bool) IValidation implementation. Same with IsAnnotated
- **ValidationMessage**: (string) This is message automaticly displayed when this is not validated. **Use this - one instead of annotationmessage**
- **IgnoreValidationMessage**: (bool) Ignores automaticly shown ValidationMessage and you can use - AnnotationMessage as custom.
- **CompletedCommand**: (ICommand) Executed when completed.


---

_Work in progres..._

## Further reading
- See [FormView](FormView.md)