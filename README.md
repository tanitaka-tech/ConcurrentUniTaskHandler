# Concurrent UniTask Handler

Provides a module for handling each sequence separately while executing UniTask in parallel.

## Usage Example

### Input wait loop
```cs
var result = await ConcurrentUniTaskHandler.Create(  
        // Effect
        ProcessTask.Create(  
            waitTask: WaitEffectRequestAsync,  
            onPassedTask: async ct =>  
            {  
                await PlayEffectAsync(ct);
                return true;  
            }),
    
        // Close
        ProcessTask.Create(  
            waitTask: WaitCloseRequestAsync,  
            onPassedTask: async ct =>  
            {  
                await CloseAsync(ct);
                return false;  
            }), 
    
        // NextScene
        ProcessTask.Create(  
            waitTask: WaitMoveNextSceneRequestAsync,  
            onPassedTask: async ct =>  
            {  
                await LoadNextSceneAsync(ct);
                return false;
            }), 
    )    
    .LoopProcessFirstCompletedTaskAsync(  
        checkNeedLoop: result => result,  
        cancellationToken: cancellationToken  
);
```

## Installation

### Install via git URL
1. Open the Package Manager
1. Press [＋▼] button and click Add package from git URL...
1. Enter the following:
    - https://github.com/tanitaka-tech/Concurrent-UniTask-Handler.git

### ~~Install via OpenUPM~~ (not yet)
```sh
openupm add com.tanitaka-tech.concurrent-unitask-handler
```

## Requirement
- [UniTask](https://github.com/Cysharp/UniTask)