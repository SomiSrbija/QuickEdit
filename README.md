# QuickEdit

QuickEdit is a powerful yet simple text editor built using C# and the .NET Framework with a rich set of features including syntax highlighting, code folding, rich text editing, terminal emulator, inline error detection, split view, customizable keyboard shortcuts, and much more. 

## Roadmap
Track bugs and features that are currently being worked/fixed on via this link [Project]https://github.com/users/SomiSrbija/projects/1/views/1

## Features

- **Syntax Highlighting**: Supports C# and other common programming languages.
- **Code Folding**: Allows you to collapse and expand code blocks for better readability.
- **Rich Text Editing**: Format text with bold, italic, underline, and color.
- **Terminal Emulator**: Run command-line operations within the editor.
- **Inline Error Detection**: Highlights syntax errors inline.
- **Split View**: View two files side-by-side.
- **Dark Mode**: Switch between light and dark themes.
- **Find and Replace**: Quickly find and replace text within the document.
- **Word Count**: Get the word count of the current document.
- **Drag and Drop**: Support for dragging text or files into the editor.
- **Customizable Keyboard Shortcuts**: Change keyboard shortcuts from the settings page.

## Getting Started

### Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.7.2 or later

### Installation

1. **Clone the repository**:

    ```bash
    git clone https://github.com/your-username/QuickEdit.git
    ```

2. **Open the solution in Visual Studio**:

    Open `QuickEdit.sln` in Visual Studio.

3. **Restore NuGet packages**:

    Restore the required NuGet packages by right-clicking on the solution in the Solution Explorer and selecting `Restore NuGet Packages`.

4. **Build the solution**:

    Build the solution by selecting `Build > Build Solution` from the menu.

5. **Run the application**:

    Start the application by pressing `F5` or selecting `Debug > Start Debugging` from the menu.

## Usage

### Main Features

#### Syntax Highlighting

QuickEdit uses ScintillaNET for syntax highlighting. The supported languages include C#, HTML, XML, and more. The editor automatically highlights keywords, strings, comments, and other elements to enhance code readability.

#### Code Folding

Code folding allows you to collapse and expand code blocks. This feature is particularly useful for managing large files. You can fold or unfold code sections by clicking the markers in the margin.

#### Rich Text Editing

The editor provides options to format text as bold, italic, underline, and change text color. These options are available under the `Format` menu.

#### Terminal Emulator

QuickEdit includes a built-in terminal emulator that allows you to execute command-line operations directly within the editor. Access the terminal from the `View` menu.

#### Inline Error Detection

QuickEdit highlights syntax errors inline using squiggly lines. This feature helps you identify and correct errors quickly.

#### Split View

Split view allows you to view two files side-by-side. Enable split view from the `View` menu to work on multiple files simultaneously.

#### Dark Mode

Switch between light and dark themes for a comfortable editing experience in different lighting conditions. Toggle dark mode from the `View` menu.

#### Find and Replace

Quickly find and replace text within the document using the `Find/Replace` option in the `Tools` menu.

#### Word Count

Get the word count of the current document by selecting the `Word Count` option from the `Tools` menu.

#### Drag and Drop

Drag text or files into the editor for easy opening and editing. QuickEdit supports both text and file drag-and-drop operations.

#### Customizable Keyboard Shortcuts

Change keyboard shortcuts from the settings page. Access the settings page from the `Tools` menu and customize shortcuts as per your preference.

## Customization

### Changing Keyboard Shortcuts

1. Open the settings form from the `Tools > Settings` menu.
2. Modify the keyboard shortcuts in the settings form.
3. Save the changes to apply the new shortcuts.

### Adding More Features

You can extend QuickEdit by adding new features. Follow these steps to add a new feature:

1. **Create a new form or component**:
   
   Add a new form or component to the project.

2. **Integrate the feature**:
   
   Integrate the feature by adding the necessary code to the main form (`Form1.cs`).

3. **Update the UI**:
   
   Update the user interface to include the new feature, ensuring it is accessible from the menus or toolbar.

4. **Test the feature**:
   
   Thoroughly test the new feature to ensure it works as expected and does not introduce any issues.

## Contributing

Contributions are welcome! Please follow these steps to contribute to QuickEdit:

1. **Fork the repository**:

    Fork the repository on GitHub.

2. **Clone your fork**:

    ```bash
    git clone https://github.com/your-username/QuickEdit.git
    ```

3. **Create a branch**:

    ```bash
    git checkout -b my-feature-branch
    ```

4. **Make your changes**:

    Make your changes to the codebase.

5. **Commit your changes**:

    ```bash
    git commit -m "Description of my changes"
    ```

6. **Push to your branch**:

    ```bash
    git push origin my-feature-branch
    ```

7. **Create a Pull Request**:

    Create a pull request on GitHub, describing the changes you made.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
