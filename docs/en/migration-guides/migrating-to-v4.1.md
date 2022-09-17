# Migrating to v4.0

InputKit version 4.1 is a major release. It has many new features and improvements. Some of them makes breaking-changes.

## What's new?
Validation logic is changed completely. Most of properties are removed and validation classes are added. You can read more about it [here](../components/controls/FormView.md#validations).

## Breaking changes

Validation is a cross-cutting concern, so lots of components are affected by this change. You should take action if you're using any of them.

### CheckBox

- `IsRequired` is removed
- `IsValidated` is renamed to IsValid
- `ValidationMessage` is removed

    New usage is like below:
    ```xml
    <input:Checkbox>
        <input:Checkbox.Validation>
            <input:RequiredValidation Message="This field is required." />
        </input:Checkbox.Validation>
    </input:Checkbox>
    ```

### RadioButton
- `IsRequired` is removed
- `IsValidated` is renamed to IsValid
- `ValidationMessage` is removed

    New usage is like below:
    ```xml
    <input:Checkbox>
        <input:Checkbox.Validation>
            <input:RequiredValidation Message="This field is required." />
        </input:Checkbox.Validation>
    </input:Checkbox>
    ```

### AdvancedEntry
- `IsRequired` is removed
- `IsValidated` is renamed to IsValid
- `Annotation` is removed
- `ValidationMessage` is removed
- `IsAnnotated` is removed
- `AnnotationColor` is renamed as ValidationColor
- `UpdateKeyboard()` method is removed
- `MinLength` is removed.
- `RegexPattern` is removed.
- `IgnoreValidationMessage`
- `ValidationChanged` event is removed.
- `Clicked` event is removed.
- `Nullable` is removed

Visit [Validations documentation](../components/controls/FormView.md#validations) for new usage information..