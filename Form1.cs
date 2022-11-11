using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Desktop_VU {
    public partial class Form1 : Form {
        private class Position {
            public Point comboPos;
            public Point progPos;
            public Point remPos;
            public Point addPos;
            public Point refPos;

            public Position(Point _comboPos, Point _progPos, Point _remPos, Point _addPos, Point _refPos) {
                comboPos = _comboPos;
                progPos = _progPos;
                remPos = _remPos;
                addPos = _addPos;
                refPos = _refPos;
            }
        }

        private class VUMonitor {
            public ComboBox comboBox;
            public ProgressBar progBar;
            public Button remBut;

            public VUMonitor(ComboBox _comboBox, ProgressBar _progBar, Button _remBut) {
                comboBox = _comboBox;
                progBar = _progBar;
                remBut = _remBut;
            }
        }

        private const int MAX_MONITORS = 5;
        private const int MAX_WIDTH = 300;

		private WaveIn monitor;
		private int numMonitors = 0;

        private MMDevice[] deviceList;
        private List<Position> usedPositions = new List<Position>();
        private List<VUMonitor> activeMonitors = new List<VUMonitor>();

		private void SetWindowSize() {
			// Set size based on position of last add button + offset
			Size minSize = new Size(MAX_WIDTH, usedPositions[numMonitors - 1].addPos.Y + 70);
			this.Size = minSize; // Set size
								 // Prevent manual scaling
			this.MinimumSize = minSize;
			this.MaximumSize = minSize;
		}

		// Refresh list of audio devices
		private void UpdateDevices() {
			// WASAPI devices
			MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
			// Grab active devices
			deviceList = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active).ToArray();
			// Update devices, and clear monitors
			foreach (VUMonitor item in activeMonitors) {
				item.comboBox.Items.Clear();
				item.comboBox.Items.AddRange(deviceList);
				item.comboBox.Text = "";
				item.progBar.Value = 0;
			}
		}

		private void ShiftMonitors(int removedIndex) {
			for (int x = 0; x < numMonitors; x++) { // Iterate through monitors
				if (x > removedIndex) { // If index past the removed monitor
					// Move each monitor element to next position
					activeMonitors[x].comboBox.Location = usedPositions[x - 1].comboPos;
					activeMonitors[x].progBar.Location = usedPositions[x - 1].progPos;
					activeMonitors[x].remBut.Location = usedPositions[x - 1].remPos;
				}
			}
		}

		// Removes a monitor, shifts the others, and resizes the window
		private void RemoveLinkedMonitor(object sender, EventArgs e) {
			Button pressedBut = (Button)sender;
			for (int x = 0; x < MAX_MONITORS; x++) { // Look through each Monitor
				if (activeMonitors[x].remBut == pressedBut) { // If clicked button found
					if (numMonitors > 1) { // Only remove if not last
					    // Remove linked monitor components
						this.Controls.Remove(activeMonitors[x].comboBox);
						this.Controls.Remove(activeMonitors[x].progBar);
						this.Controls.Remove(activeMonitors[x].remBut);

						addMonitor.Show();                                                  // Display add button again
						ShiftMonitors(x);                                                   // Shift each monitor
						activeMonitors.RemoveAt(x);                                         // Remove instance of monitor
						numMonitors--;                                                      // Deduct monitor count
						addMonitor.Location = usedPositions[numMonitors - 1].addPos;        // Update add position
						refDevices.Location = usedPositions[numMonitors - 1].refPos;        // Update ref position
						SetWindowSize();                                                    // Update window scale
					}
					break;
				}
			}
		}

		private void AddMonitor() {
			ComboBox comboBox = new ComboBox();
			ProgressBar progBar = new ProgressBar();
			Button remBut = new Button();

			comboBox.Items.AddRange(deviceList);
			// PROPERTIES
			// Setting positions
			comboBox.Location = usedPositions[numMonitors].comboPos;
			progBar.Location = usedPositions[numMonitors].progPos;
			remBut.Location = usedPositions[numMonitors].remPos;
			// Sizing
			comboBox.Size = new Size(200, 20);
			progBar.Size = new Size(200, 10);
			// Styling
			progBar.Style = ProgressBarStyle.Continuous;
			remBut.Size = new Size(20, 20);
			remBut.Text = "-";
			// Add event
			remBut.Click += new EventHandler(RemoveLinkedMonitor);

			// Create and store instance of monitor
			VUMonitor thisMonitor = new VUMonitor(comboBox, progBar, remBut);
			activeMonitors.Add(thisMonitor);

			// Draw monitor onto screen
			this.Controls.Add(comboBox);
			this.Controls.Add(progBar);
			this.Controls.Add(remBut);
			// Increment num devices
			numMonitors++;
		}

		public Form1() {
            InitializeComponent();
            UpdateDevices(); // Grab devices

            // Generate positions
            int posY = 30; // Init vertical spacing
            for (int x = 0; x < MAX_MONITORS; x++) { // Loop through each monitor position
                // Create and store each comboPos, progPos, remPos, addPos, refPos
                Position pos = new Position(new Point(20, posY), new Point(20, posY + 30), new Point(240, posY + 10), new Point(30, posY + 50), new Point(150, posY + 50));
                usedPositions.Add(pos);
                posY += 50; // Increment vertical distance
            }

            AddMonitor(); // Add single monitor by default
            // Setup add button
            addMonitor.Size = new Size(60, 25);
            addMonitor.Location = usedPositions[0].addPos; // Use first/top monitor position
            // Setup ref button
            refDevices.Size = new Size(60, 25);
            refDevices.Location = usedPositions[0].refPos; // Use first/top monitor position

            SetWindowSize(); // Setup window scaling
            // Monitor audio lines
            monitor = new WaveIn();
            try {
				monitor.StartRecording();
			} catch (Exception _badDevice) { // Couldn't find out how to target _badDeviceId error..
				Console.WriteLine(String.Format("{0} Error Initializing", _badDevice.Message));
				return;
			}
        }

        private void addMonitor_Click(object sender, EventArgs e) {
            if (numMonitors < MAX_MONITORS) { // If MAX_MONITORS not reached
                AddMonitor(); // Add monitor
                SetWindowSize(); // Rescale
                addMonitor.Location = usedPositions[numMonitors - 1].addPos; // Update add button pos
                refDevices.Location = usedPositions[numMonitors - 1].refPos; // Update add button pos
                if (numMonitors >= MAX_MONITORS) { // Hide add button if MAX_MONITORS reached, and shift refresh button
					addMonitor.Hide(); 
                    refDevices.Location = new Point(60, 280); 
                }
            }
        }

		private void refDevices_Click(object sender, EventArgs e) {
			UpdateDevices();
		}

        private void timer1_Tick(object sender, EventArgs e) {
            foreach (VUMonitor active in activeMonitors) {
				MMDevice device = (MMDevice)active.comboBox.SelectedItem;
                if (device != null) {
                    // Grab volume level of device 0-1
                    float level = device.AudioMeterInformation.MasterPeakValue;
                    // Returns number from 0 to "-infinity"
                    float ApproxDB = Convert.ToSingle(Math.Log10(level) * 10);
                    // Floors value to min/max
                    if (ApproxDB < -40) ApproxDB = -40;
                    else if (ApproxDB > 0.5f) ApproxDB = 0;
                    // Colouring bar
                    if (ApproxDB > -4) { 
                        active.progBar.ForeColor = Color.Red; 
                        Thread.Sleep(200); // Very unsure if worth using
					}
                    else if (ApproxDB > -18) active.progBar.ForeColor = Color.Orange;
                    else if (ApproxDB < -18) active.progBar.ForeColor = Color.Green;
                    float percent = 1f - (Math.Abs(ApproxDB) / 40);
                    active.progBar.Value = (int)(percent * 100);
                }
            }
        }
    }
}
