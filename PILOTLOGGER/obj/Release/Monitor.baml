   M S B A M L   `   `   `  ���� F  BPILOTLOGGER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PILOTLOGGERPILOTLOGGER  U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf d `Microsoft.Maps.MapControl.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPFMicrosoft.Maps.MapControl.WPF X TLiveCharts.Geared, Version=1.2.8.2, Culture=neutral, PublicKeyToken=7292f245d14ab8bfP:clr-namespace:LiveCharts.Geared;assembly=LiveCharts.GearedLiveCharts.Geared     PILOTLOGGER.Monitor   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:PILOTLOGGER     Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     `mRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPF     c _Microsoft.Expression.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35=ed4http://schemas.microsoft.com/expression/2010/drawing  X	 THelixToolkit.Wpf, Version=2.12.0.0, Culture=neutral, PublicKeyToken=52aa3500039caf0d/HelixToolkithttp://helix-toolkit.org/wpf 	 Ggeared:clr-namespace:LiveCharts.Geared;assembly=LiveCharts.Geared 	   � Title$  Monitor��$
��1268��$
��2252�����	#FF2C2C2C � WindowState$ 	Maximized=� � WindowStyle$ SingleBorderWindow=� � 	Resources    DroneLocationTemplate �� 
TargetType+  %Microsoft.Maps.MapControl.WPF.Pushpin �Name��%  &	        �� "  .R�� $ ContentGrid��$��Center=�$��Center=�$y�Hidden=�.+��� .+�� $��0q�$��20��$��20��.+�A� ���#00A4EF���	#FF005167�� $��Center=�$��Center=�#��z��#��z��$��0q�$��Segoe UI)�$
��Bold"����	#FFB8D30B.��(� .��� $��5,5,5,5q�$	��990��$
��1780��.+��� $
��Left=�$	��142��$��0,10,0,0q�$	��Top=�$	��381��$��newlogo.png��� -   $ darkGrid��$��5,5,5,5q�$	��990��$
��1780�� K� ZIndex$	 100��$y�hidden=�.+�� $	��990��$
��1780��$	 100�� J� Opacity$ .5Q����#000000�� $��Reconnecting��$��589,299,573,529q�$	 110��$	 1.0Q�.���� $��ATTEMPTING RECONNECT��$
��Left=�$��
162,37,0,0q�$	��Top=�$��56��$	��275��$��22(��� $��3D Model View��$��417,651,1062,28q�$ 10�����	#E51F1F1F.���  � ColumnDefinitions �� $
��173* ��� $
��135* ��� $	��41* �.+��� $
��Left=�$
��1102��$	��Top=�$
��1148��$��grid.png��$��-434,80,-429,-939q�$��3��& 	  HelixToolkit.Wpf.HelixViewport3D  -   	  Name$	 Viewport��
   ZoomExtentsWhenLoaded
 . $��3��$��0,0,0,10q�   Children.  	 HelixToolkit.Wpf.SunLight  g� -   
Models���� $��Alignment Metrics��$��0,316,1389,348q�.���  �� $
��225* ��� $
��124* �.+��� $��HEADING��$	��Top=�$��63,157,122,0q�� $��10,0,71,121q�.+�� $��Center=�$��Center=�.+�%  Microsoft.Expression.Shapes.Arc     
StartAngle$
 -180Q�   EndAngle$	 179Q� �� Stretch$
 None=�$	��130��$	��130��$��15�� �� StrokeDashArray$	 .25R����	#FF484D5F  $ -1Q�$ 1Q�$
 None=�$	��130��$	��130��$��15��$	 .25R�
���  ��$	 100��  $
 None=�$	��130��$	��130��$��15��$	 .25R�
�����@� ��  �� Path$ HeadingLowerLimit� �� $ HeadingLowerLimit��� $��Center=�$��Center=�.+��� ���	#FF878A9F$��Center=�$��10(�N�� -    ��Name$ 
headingval��
�������$��Center=�$��30(�$��Light"����� $ Heading�� $��199,0,0,121q�$��2��.+�� $��Center=�$��Center=�.+�  $
 -180Q�$	 179Q�$
 None=�$	��130��$	��130��$��15��$	 .25R����	#FF484D5F  $ -5Q�$ 5Q�$
 None=�$	��130��$	��130��$��15��$	 .25R�
���  ��$	 100��  $
 None=�$	��130��$	��130��$��15��$	 .25R�
�����@� �� $ RollLowerLimit� �� $ RollUpperLimit��� $��Center=�$��Center=�.+��� -   $ rollval��
�������$��Center=�$��30(�$��Light"����� $
 Roll�� $��97,111,102,10q�$��2��.+�� $��Center=�$��Center=�.+�  $ 0Q�$
 -175Q�$
 None=�$	��130��$	��130��$��15��$	 .25R����	#FF484D5F  $ 85Q�$ 95Q�$
 None=�$	��130��$	��130��$��15��$	 .25R�
���  ��$	 100��  $
 None=�$	��130��$	��130��$��15��$	 .25R�
�����@� �� $ PitchLowerLimit� �� $ PitchUpperLimit��� $��Center=�$��Center=�.+��� -   $ pitchal��
�������$��Center=�$��30(�$��Light"����� $ Pitch��� $
��ROLL��$	��Top=�$��1��$��31,157,74,0q��� $��PITCH��$	��Top=�$��170,262,36,0q��� $��Live GPS Monitoring��$��396,10,10,10q�.��� .+�'  !Microsoft.Maps.MapControl.WPF.Map  -     Name$ MainMap��+  %Microsoft.Maps.MapControl.WPF.MapCore   CredentialsProviderT  NMicrosoft.Maps.MapControl.WPF.Design.ApplicationIdCredentialsProviderConverter$F @Am8fO-_nP0e8k7HcIDnOG7JzvbNlc2rMdGYmF3-5Yp0WfgyVqz7MevsWFcq4ze3k    Mode;  5Microsoft.Maps.MapControl.WPF.Design.MapModeConverter$ Aerial $��0,0,0,-2q��� $��Pilot Dashboard��$
��Left=�$	��184��$	��Top=�$
��1021��$��327,728,0,0q�$
 0.97Q����	#E51F1F1F.���  �� $
��108* ��� $
��979* �.+�� � $��
10,0,819,3q�$��2��.+��� $��ALIGNMENT METRICS��$
��Left=�$��
10,0,-48,0q�$	��Top=�$	��198��$��16(��� $��ROLL:��$��10,34,30.852,63q��� $
��Left=�$��	95,32,0,0q�$	��Top=�$
��Bold"�$��14(�$��2��
�����@����� $
 Roll��� $��PITCH:��$��10,64,21,43q��� $
��Left=�$��	95,63,0,0q�$	��Top=�$
��Bold"�$��14(�$��2��
�����@����� $ Pitch��� $
��YAW:��$��10,92,21,15q��� $
��Left=�$��	95,90,0,0q�$	��Top=�$
��Bold"�$��14(�$��2��
�����@����� $	 Yaw�� $��125,10,642,0q�$��1��.+�� $��Center=�$��Center=�$��8,-2,0,0q�$	��115��.+�  $
 -120Q�$	 120Q�$
 None=�$	��130��$	��130��$��15��$	 .25R����	#FF484D5F  $
 -120Q�$
 None=�$	��130��$	��130��$��15��$	 .25R�
�����@� �� $ VelocityCurve��� $��Center=�$��Center=�.+��� $	��mph�����	#FF878A9F$��Center=�$��10(��� -   $ speedval��
�������$��Center=�$��30(�$��Light"����� $ Velocity�� $��299,13,468,3q�$��1��.+�� $��Center=�$��Center=�$��8,-2,0,0q�$	��115��.+�  $
 -120Q�$	 120Q�$
 None=�$	��130��$	��130��$��15��$	 .25R����	#FF484D5F  $
 -120Q�$
 None=�$	��130��$	��130��$��15��$	 .25R�
�����@� �� $ AccelerationCurve��� $��Center=�$��Center=�.+��� ���	#FF878A9F$��Center=�$��10(�m/s��  �� BaselineAlignment$ Superscript=�.�2�� -	   $ accelval��
�������$��Center=�$��30(�$��Light"����� *Acceleration+� $��471,10,296,0q�$��1��.+�� $��Center=�$��Center=�$��8,-2,0,0q�$	��115��.+�  $
 -120Q�$	 120Q�$
 None=�$	��130��$	��130��$��15��$	 .25R����	#FF484D5F  $
 -120Q�$
 None=�$	��130��$	��130��$��15��$	 .25R�
�����@� �� $ AltitudeCurve��� $��Center=�$��Center=�.+��� $��meters�����	#FF878A9F$��Center=�$��10(��� -
   $ altval��
�������$��Center=�$��30(�$��Light"����� $ Altitude�� $��635,13,132,3q�$��1��.+�� $��Center=�$��Center=�$��8,-2,0,0q�$	��115��.+�  $
 -120Q�$	 120Q�$
 None=�$	��130��$	��130��$��15��$	 .25R����	#FF484D5F  $
 -120Q�$
 None=�$	��130��$	��130��$��15��$	 .25R�
�����@� �� $ AltitudeCurve��� $��Center=�$��Center=�.+��� $��meters�����	#FF878A9F$��Center=�$��10(��� -   $ sealevelaltval��
�������$��Center=�$��30(�$��Light"����� $ Altitude��� $��VELOCITY��$
��Left=�$��169,117,0,0q�$	��Top=�$��1���� $��ACCELERATION��$
��Left=�$��331,120,0,0q�$	��Top=�$��1���� $��ALTITUDE��$
��Left=�$��519,120,0,0q�$	��Top=�$��1���� $
��Left=�$��	10,47,0,0q�$	��Top=��� $��REL ALTITUDE��$
��Left=�$��671,117,0,0q�$	��Top=�$��1���� $��Live Data Stream��$��0,647,1389,12q�.���  �� $
��139* ��� $
��147* ��� $
��477* �.+�#	  LiveCharts.Wpf.CartesianChart	  -    	 Name$ chart��$��
10,0,10,10q�$��3��&
   LiveCharts.Wpf.Charts.Base.Chart 
  Series �� *chartSeries+�� $��PILOT RC Groundstation��$��0,167,1389,679q�.��� .+��� -    ��Name$ 
graphcombo��$
��Left=�$��	10,29,0,0q�$	��Top=�$	��193�� J� RenderTransformOrigin$ 0.617,-0.333&��� $ ��TOGGLE LIVE MONITOR VALUE:��$
��Left=�$��10,8,0,0q�$	��Top=�$	��193���� -   $��Kill Monitor��$
��Left=�$��
267,65,0,0q�$	��Top=�$��98��