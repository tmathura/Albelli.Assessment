# .NET Software Engineer Technical Assignment

## What is this repository about?

This is repository contains the solution to the business problem that was given by albelli to assess how I build testable and maintainable software with design and architecture in mind using industry best practices.

## Business flow

A customer can order 1 or multiple items. For example, an order could consist of a photo book and 2 canvases. After the order is produced, it is delivered to one out of thousands of pickup points across the country. The package is put in a bin on a shelf at the pickup point. The bin has to be sufficiently wide for the package. Since bins are reserved upfront, we need to calculate the minimum bin width required for the order at the moment of order creation.  

## Assignment

Create a .NET Web API that accepts an order, stores it, and responds with the minimum bin width. We also should be able to get back all the information that is known about the order by its ID. Cover code with tests where you find it important. More information on this assesment can be found [here](https://github.com/albumprinter/dotnet-engineer-assignment/blob/master/README.md).

## Status

[![Build Status](https://dev.azure.com/tmathura/Albelli.Assessment/_apis/build/status/tmathura.Albelli.Assessment?branchName=main)](https://dev.azure.com/tmathura/Albelli.Assessment/_build/latest?definitionId=4&branchName=main)
[![Commits](https://img.shields.io/github/commit-activity/w/tmathura/Albelli.Assessment.svg?style=flat-square)](https://github.com/tmathura/Albelli.Assessment/commits/main)
[![GNU GPL v3](https://img.shields.io/badge/license-GNU%20GPL%20v3-blue.svg?maxAge=60&style=flat-square)](http://www.gnu.org/licenses/gpl.html)
[![Copyright 2010-2017](https://img.shields.io/badge/copyright-2022-blue.svg?maxAge=60&style=flat-square)](https://github.com/tmathura/Albelli.Assessment)

## Configuring the Development Environment

### Requirements

* [Visual Studio Community 2022](https://www.visualstudio.com/vs/community/)
* [Git](https://git-scm.com/downloads)

### Setup

* Make sure all the required software mentioned above are installed
* Clone the repository into your development machine ([*info*](https://help.github.com/desktop/guides/contributing/working-with-your-remote-repository-on-github-or-github-enterprise))

### Development

* Open `Albelli.Assessment.sln` in Visual Studio 2022.
* Make sure `Albelli.Assessment.WebApi` is set as the startup project.
* Press `F5`.