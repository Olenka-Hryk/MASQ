# MASQ - Metric Analyzer Software Quality 
![](/Readme images/github-readme-image-emblem.png)

>As a result of the analysis of the current state of evaluation of software quality, as well as available tools and methods for its evaluation, it was found that there are a large number of them, but they are all separate and not unified. Therefore, there is a need for their generalization and development of a software tool that takes into account the influence of various factors in quality management.

***MASQ*** - is a software tool for determining the [software quality] by methods of [metric analysis], which provides the ability to predict its further successful development and formation of the corresponding set of data.

#
**KEY POSSIBILITIES:**
  - Сalculates [software metrics with accurate and predicted values] at the design stage,  by calculation and expert methods
  - Analysis and processing metric estimates
  - Providing recommendations and conclusions
  - Predicting the success of development investigated software 
  - Finding a [ANN, artificial neural networks] plural for a quantitative quality assessment
  - Graphic representation of results


## Table of Contents
- [Getting Started](#markdown-header-getting-started)
  - [Clone the Repository](#markdown-header-clone)
  - [Prerequisites](#markdown-prerequisites)
  - [Installation](#markdown-header-installation)
- [Built With](#markdown-header-built-with)
- [UML Diagrams](#markdown-header-uml-diagrams)
  - [Class Diagram](#markdown-header-class-diagram)
  - [Sequence Diagram](#markdown-header-sequence-diagram)
  - [Use Case Diagram](#markdown-header-use-case-diagram)
- [Project Screen Shots](#markdown-header-project-screen-shots)
- [Video Reporting](#markdown-header-video-reporting)
- [Author](#markdown-header-author)


<a name="markdown-header-getting-started"></a>
## Getting Started

<a name="markdown-header-clone"></a>
#### Clone the Repository
 As usual, you get started by cloning the project to your local machine:
 ```sh
 $ git://github.com/
 ```
 
 
<a name="markdown-prerequisites"></a>
#### Prerequisites 
1. .NET Core SDK 2.1 (v. 2.1.400) or later versions.
2. Microsoft Visual Studio .NET 2017
3. Git


<a name="markdown-header-installation"></a>
#### Installation
The easiest way to use these programm without using `Git` is to download the `zip file` containing the current version. You can then unzip the entire archive and use this program in `Visual Studio .NET 2017`.

For more info about the programming models, platforms, languages, and APIs demonstrated in this app, please refer to the guidance available in [MSDN].



<a name="markdown-header-built-with"></a>
## Built With
Development environment for this application is `Microsoft Visual Studio .NET 2017`.

This project use the following `libraries`:
* LiveCharts.Wpf 0.9.7


Used `technologys` and `tools`:
* [Microsoft Visual Studio .NET 2017] - for develop application
* [WPF .NET] - for development a modern user interface
* [NuGet Package Manager] - provide the ability to consume packages
* [LiveCharts] - for data visualization



<a name="markdown-header-uml-diagrams"></a>
## UML Diagrams

<a name="markdown-header-class-diagram"></a>
#### Class Diagram
![](/Readme UML/github-readme-image-class-diagram.png)

<a name="markdown-header-sequence-diagram"></a>
#### Sequence Diagram
![](/Readme UML/github-readme-image-sequence-diagram.png)

<a name="markdown-header-use-case-diagram"></a>
#### Use Case Diagram
![](/Readme UML/github-readme-image-use-case-diagram.png)



<a name="markdown-header-project-screen-shots"></a>
## Project Screen Shots
Get started with the program ***MASQ***. The initial application splash-window:
![](/Readme images/github-readme-image-start-window.png)

The main window of the program consists of a toolbar, a list of metrics, a workspace, a user logging area, a resulting area:
![](/Readme images/github-readme-image-example-find-metric-1.png)

The working area of the quality metrics for each metric varies depending on the way of finding. For example, finding a *DP quality metric with predicted values* has such a workspace:
![](/Readme images/github-readme-image-example-find-metric-2.png)

Also, there are metrics that are solved in several stages and use certain algorithms. Then there is a logical switch between the windows in search of the correct result. All results such metrics are submitted in formulas:
![](/Readme images/github-readme-image-example-find-result.png)

Besides the main features of the program there are additional options. Search for a specific metric, search for a parameter, enter the initial data, user manual, and so on:
![](/Readme images/github-readme-image-all-possibility-in-use.png)

The graphical representation of data takes place in three stages: tabular, histogram, graph. Also based on the results the program generate conclusion:
![](/Readme images/github-readme-image-graphic-representation-of-results.png)

The complete user manual can be found [here].

<a name="markdown-header-video-reporting"></a>
## Video Reporting
[![Watch the video]()](/Readme images/MASQ-video-reporting.mp4)


<a name="markdown-header-author"></a>
## Author
- ****Olena Andrushchakevych**** - olena.andrushchakevych@gmail.com



[software quality]: <https://en.wikipedia.org/wiki/Software_quality>
[metric analysis]: <https://project-management-knowledge.com/definitions/q/quality-metrics/>
[software metrics with accurate and predicted values]:<https://www.uad.lviv.ua/uploads/2018/vchenarada/govorushchenkodis.pdf>
[ANN, artificial neural networks]:<https://en.wikipedia.org/wiki/Artificial_neural_network>
[MSDN]:<https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms746927(v=vs.100)>
[Microsoft Visual Studio .NET 2017]:<https://visualstudio.microsoft.com/ru/vs/features/net-development/?rr=https%3A%2F%2Fwww.google.com.ua%2F>
[WPF .NET]:<https://docs.microsoft.com/ru-ru/previous-versions/dotnet/netframework-4.0/aa970268(v=vs.100)>
[NuGet Package Manager]:<https://marketplace.visualstudio.com/items?itemName=NuGetTeam.NuGetPackageManager>
[LiveCharts]:<https://lvcharts.net/>
[here]:<MASQ (C#)/Instruction for use MASQ>