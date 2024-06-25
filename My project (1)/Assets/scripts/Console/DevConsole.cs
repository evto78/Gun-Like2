using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevConsole
{
    private readonly string prefix;
    private readonly IEnumerable<IConsoleCommand> commands;
    public DevConsole(string prefix, IEnumerable<IConsoleCommand> commands)
    {
        this.prefix = prefix;
        this.commands = commands;
    }

    public void ProcessCommand(string commandInput, string[] args)
    {
        foreach (var command in commands)
        {
            if (!commandInput.Equals(command.CommandWord, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            if (command.Process(args))
            {
                return;
            }
        }
    }
}
