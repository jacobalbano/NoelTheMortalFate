Project setup:
- Create a directory to use as your game root
	If running in Visual Studio, this path must be given as a debug argument for each application in the suite
	If running standalone, the applications will use the current folder as the environment root
	
Applications:

# Noel.Setup
Should be the first tool you run. This will establish the environment, set up folder structure, and download/extract all current seasons of 被虐のノエル/Noel the Mortal Fate into their appropriate folders

# Noel.Extract
Explores game files in each season, extracting patchable strings into a working directory. Performs a backup of every patchable file

# Noel.Edit
Launches a tiny server that hosts a web app and REST api which allows extracted strings to be edited

# Noel.Patch
Applies changes from working data back into the game files

# Noel.Launcher
Displays a graphical interface that allows each individual game season to be launched (a replacement for the Unity-based launcher that ships with the Steam release)

-- 

