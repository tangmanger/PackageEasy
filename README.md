# Based on NSIS packaging visualization tool


## Original intention of development
* The original HM NIS Edit is very simple to create and install, but it is not easy to modify, so it is newly added.
* The original nsi script directly adds components, or deletes components, deletes files, etc. It is not simple to operate.
* The most important thing is that I want to do something for fun~ Haha
## Software Architecture
  It is developed using WPF+MVVM, imitating the Visual Studio style (although the imitation is not that thorough -_-!). It is developed based on .net 6 and supports simultaneous editing of multiple projects. The binding file format is __.pge__ file format. At present, there are still many business scenarios that are not covered, and there are still some minor bugs, which will be slowly maintained and improved in the future.

  # README.md
- en [English](Readmes/README.md)
- zh_CN [简体中文](Readmes/README.zh_CN.md)

## Release Notes
### V1.0.5
* Solve the problem of multi-language switching
* To solve the installation problem, you can choose to copy files to the specified directory
* Solve the problem of being unable to configure NSIS path
* Solve the problem that the compilation cannot pass when the monitoring process is not set on the end page
* Added batch deletion, batch selection, and batch change directory
* Added a new ignore list to solve the problem of repeatedly adding unnecessary files
* [Update instructions](https://mp.weixin.qq.com/s/8c4ONmWgJ8Uw-Q9E6EYt-A)
### V1.0.4
* There is a compilation problem when the generated script name has spaces.
* Added pky encryption format, supports file password protection, and supports pge->pky->pge seamless switching.
* Added relative path mode
* Added save as
* Added new compilation shortcut key Ctrl+B
* Added selective copy strategy
* Improve information about the interface
* Modify the import folder strategy and import the entire workspace
* Modify detail row to show additional strategies
* Added NSIS component, which can be installed with one click. Remember if you use the detection process, please remember to install the process detection plug-in in the extension.
* [Update instructions](https://mp.weixin.qq.com/s/1kKiUmz7wdZkE0rTVOCSiA)
### V1.0.3
**The future version will be changed to 3 digits, the design of the previous version was wrong - - Ashamed**
* There is a compilation problem when the generated script name has spaces.
* Registration format can appear as an optional component
* The control panel can choose whether to display the version
* Can add folders
* Strip base setup program name format registration
* When opening the file after copying, you need to select the path to save it.
* Fixed the issue where the process still continues after the uninstallation is cancelled.
* In multi-language mode, select different files or dlls. For example: in English mode, you can select the English configuration file. In Chinese mode, you can select the Chinese configuration file.
* Fixed silent installation script and incorrect installation
* Added export script, open script
* Added internal version records to files for traceability
* [Usage introduction method](https://mp.weixin.qq.com/s?__biz=MzA5ODY4MDkzOA==&mid=2447903959&idx=1&sn=7ecb538442d049d320706601ece30371&chksm=849145d2b3e6ccc4d256d83a95f6b350858affd0e5cb29c26a8066588b4d1f66c0fc91d1d7a9&token=886432174&lang=zh_CN#rd)
### V1.2
* Set the log format to utf8
* Added compilation progress animation
* Added file version
* Added theme switching
* Added new control panel to display installation package size
* Other bug fixes & UI adjustments
### V1.1
* Added data verification
* Added the ability to turn off table page verification
* Added attribute value change detection
* Added production company name
* Added installation package publisher display
* Optimize plug-in installation
* Other minor UI issues adjusted
### V1.0
* Support custom project creation and installation
* Support file format customization
* Support process detection and installation prompts
* Support visual editing of components
* Support packaged plug-in installation
* Support quick access to recently opened files
* Support verification before compilation
*There are many more~
## Instructions
There will be a tutorial later. Thanks for watching~
[How to use](https://mp.weixin.qq.com/s?__biz=MzA5ODY4MDkzOA==&mid=2447903933&idx=1&sn=5f6107ae0bea22ad1f7c0eb7d81fe70d&chksm=849145b8b3e6ccaef109b2a387560e4ef9e69b22f44e138b6645aeb958a1384c03449413b362#rd)
