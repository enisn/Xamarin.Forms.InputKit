# Migrating to v4.1
InputKit version 4.1 comes a couple of breaking-changes. Please check following list if you're upgrading to v4.1.

## Breaking changes

Xamarin Forms 4.1 isn't released. So there is no breaking-change for Xamarin Forms.

### AdvancedEntry
- `IconColor` is removed from `AdvancedEntry`.
- `IconImage` property type is changed to `ImageSource` from `string`. _(Not FontImageSource can be passed as parameter.)_

> After this change, AdvancedEntry can be used on Windows platform with all features.

### IconView

- IconView is deprecated. Use `IconImage` property of `AdvancedEntry` instead.
- IconView won't be part of this package in next versions. Please consider to change it FontImage or something equivalent.

IconView one of the platform specific limitations in InputKit. With this removal, Windows platform support is became more stable.


> It's still working in `v4.1` but it will be removed in next versions.