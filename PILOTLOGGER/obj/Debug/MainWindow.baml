   M S B A M L   `   `   `  ����F  BPILOTLOGGER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PILOTLOGGERPILOTLOGGER  5   	       PILOTLOGGER.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:PILOTLOGGER     5   	     � Title$  PILOT LOGGER��5   	   $��480.6��6   $��415.6��6-   ���	#FF2C2C2C.��5	      � $��0,0,-13.6,-17q�6   .+�5
   
   �� $��PILOT Logger��6   $
��Left=�6)   $	��462��6D   $	��Top=�6Q   $	��416��6i   .��5      � .+�5      �� -   5        $��Add New��5   .   $
��Left=�6@   $��
265,71,0,0q�6[   $	��Top=�6o   $��98��6�   �� 5      -   5         ��Name$ schemacombo��5      $
��Left=�60   $��	20,71,0,0q�6K   $	��Top=�6^   $	��226��6v   �� 5      -   5         ��Name$ 	outputBox��5      $
��Left=�6-   $��23��6H   $��
20,144,0,0q�6T    �� TextWrapping$
 Wrap=�6h   $	��Top=�6|   $	��226��6�   {�.  $��8(�6�   �� 5      -   5        $��Open Folder��5   ,   $
��Left=�6B   $��265,144,0,0q�6]   $	��Top=�6r   $��98��6�   �� 5      $��Serial Data Schema��6   $
��Left=�65   $��
134,47,0,0q�6P   $	��Top=�6d   �� 5      $��Log Output Directory��6   $
��Left=�67   $��124,118,0,0q�6R   $	��Top=�6g   �� 5      $��COM Port��6   $
��Left=�6+   $��164,191,0,0q�6F   $	��Top=�6[    J� RenderTransformOrigin$ 0.564,2.938&�6s   �� 5      -   5        $ comcombo��5      $
��Left=�6-   $��
85,221,0,0q�6H   $	��Top=�6\   $	��226��6t   �� 5      -   5         ��Name$ startBut��5      $��Start Logging��6C   $
��Left=�6[   $��
10,324,0,0q�6v   $	��Top=�6�   $	��149��6�   $��70��6�   {�.  6�   �� 5      -   5        $ stopBut��5      $��Stop Logging��6@   $
��Left=�6W   $��241,324,0,0q�6r   $	��Top=�6�   $	��149��6�   $��70��6�   {�.  6�   �� 5      $��STATUS:��6   $
��Left=�6*   $��116,283,0,0q�6E   $	��Top=�6Z   $��55��6r   $
��Bold"�6}   �� 5      -   5         ��Name$ statusLabel��5      $��NOT CONNECTED��6-   $
��Left=�6E   $��176,283,0,0q�6`   $	��Top=�6u   $	��146��6�   5      5      5      5      