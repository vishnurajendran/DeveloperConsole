# DeveloperConsole
 A simple developer console for unity

![](https://github.com/vishnurajendran/DeveloperConsole/blob/main/devconsole.png)

## Installation
 - Downlad and import the unity package.
 - Drag and drop the DevConsole prefab into your scene

## Add Commands
 - Make a static function that takes a string[] parameter
 - Add the [ConsoleCommand] attribute to it.

###Example with return statement
```
[ConsoleCommand("description", "usage")]
public static string myMethod(string[] args)
{
  return "myMethod Called";
}
```
###Example without return statement
```
[ConsoleCommand("description", "usage")]
public static string myMethod(string[] args)
{
  Debug.Log("myMethod called");
}
```
