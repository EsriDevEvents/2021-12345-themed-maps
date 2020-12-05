# Theme-aware map specification

This document describes a pattern for creating and reading **theme-aware maps**.

## Expectations for maps

* Themes are defined as top-level group layers
* Each top-level theme group layer has a name starting with `Theme: `
* Only one top-level group layer is visible by default

## Expectations for theme-aware applications

* The following semantic themes at minimum are recognized:
    * `Light`
    * `Dark`
    * `High Contrast`
* The application can tolerate additional themes
    * If a recognized theme is present, unexpected/unrecognized group layers with names starting with `Theme: ` can be ignored
    * If no recognized theme is present, theme adaptivity is disabled (fall back to non-theme-aware behavior)
* When a theme is selected, all other themes are hidden/deselected (ideally in a way that prevents layers from making requests/rendering)

## Notes and discussion

* Additional themes are supported for branding & flexibility purposes. Consider field maps for use by an organization of astronomers; high contrast white may be used in the field during the day, with an [all-red theme used at night](https://archive.briankoberlein.com/2015/04/08/blinded-by-the-light/index.html).