# Kentico Xperience 13 - Multiple Page Selector

In Kentico Xperience 13's page builder, you get access to a [great editing component](https://docs.xperience.io/developing-websites/page-builder-development/selectors-for-page-builder-components#Selectorsforpagebuildercomponents-Pageselector) that allows you to select multiple pages on a widget property.

Unfortunately this component is restricted to only the page builder. It is extremely useful for widgets, but is not available for other areas of the administration portal such as Page Type fields.  These other areas are still based on Portal Engine/WebForms controls, which means you cannot access the page builder components.

NetConstruct created the **Multiple Page Selector** form control to bridge this gap. This form control allows you to add and remove multiple pages to a Page Type field. It also gives you the ability to reorder them.

## How to Use?

1. In Kentico, go to the Sites application
2. Import the [package](./cms_formusercontrol_XperienceCommunity_MultiplePageSelector_20230218_1212.zip)
3. In your **CMSApp** solution, add [these files](./CMS/CMSFormControls/MultiplePageSelector) into the folder `CMS/CMSFormControls/MultiplePageSelector`
4. Build the solution
5. The **Multiple Page Selector** form control should now be available on Page Types, Custom Tables, Controls and WebParts for data types **Long text** and **Text**

## Contributing
If you find something wrong with the code, find a bug, or are having problems using the form control, then please open an Issue.

If you would like to fix a bug yourself, improve the code, or add more features then please create a Pull Request.
