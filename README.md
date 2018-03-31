# CLIfy
Lib for fast building command line application with C#.

This lib is still under developing. There may have some bugs, issues or contribution is welcome.

## Quick Start

Define static methods
```c#
static void Test(string path,[CLIOptional("s")] string scheme ="default",[CLIOptional("v")] bool verify = false)
{
    Console.WriteLine($"test exec path:{path} scheme={scheme} verify={verify}");
}

static CLIResult Add (int a,int b)
{
    return CLIResult.Success(a + b);
}
```

Create `CLIApp` objects, then register methods.
```c#
using CLIfy;
//...
static void Main(string[] args)
{
    CLIApp app = new CLIApp("CLIfySample",false,CLIstyle.Unix);
    app.Register<string,string,bool>(Test);
    app.RegisterFunction<int,int,CLIResult>(Add);
    app.Run(args);
}
```

Run the program then type some commands.
```console
> add 10 20
30
```
```console
> test D:\file.txt -v -s release
test exec path:D:\file.txt scheme=release verify=True
```
```console
> test -v
Param 'path' is required.
usage:     Test <path> [-s <scheme>] [-v]
```

## Features

### CLI styles (Unix-like,DOS-like)

Unix-like `test <path> -s <scheme> -v` </br>
DOS-like  `test <path> /s <scheme> /v` or `test <path> /s:<scheme> /v`

- Optional parameters

Attatch `CLIOptional` attribute to method parameter
Optional parameter must have default values.
usage: 
```c#
void function(int c,[CLIOptional("s")] string scheme ="default"){}
```
### Error analysis

Command not found
```console
> func1
Command not found 'func1'
```
Missing required parameter
```console
> test
Param 'path' is required.
usage:     Test <path> [-s <scheme>] [-v]
```

Missing value of optional parameter
```console
Optional parameter 's' missing values.
usage:     Test <path> [-s <scheme>] [-v]
```

Parameter parsing failed
```console
> add 10 str
Can not convert 'str' to parameter [b(System.Int32)]
```

### Strict mode parsing
Loose Mode
```console
> add 10 20
30
> Add 10 20
30
``

### Autogen help infomation
print help infomation with empty input
```console
Commands:
    Test <path> [-s <scheme>] [-v]
    Add <a> <b>
```

## TODO
- [ ] Fix critical bugs then release nuget package.
- [ ] API for Exit(CTRL+C), Clear(cls or clear).
- [ ] UnitTest cover
- [ ] Array type parameter Support.
- [ ] Basic expression Support.



## License
MIT
