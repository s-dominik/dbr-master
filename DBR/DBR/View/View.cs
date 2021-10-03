using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using DBR.Model;
using DBR.Persistance;
using System.Diagnostics;

namespace DBR.View
{
    public partial class View : Form
    {
        #region Fields

        private DataAccess _dataAccess;// adatelérés
        private GameModel _model; // játékmodell
        private System.Windows.Forms.Timer _timer; // időzítő

        private BackgroundWorker _gameLoopWorker = null;

        private int _tileSize = 0;
        private int _peopleSpeedMultiplier = 0;

        private PointF _cameraPosition;
        private float _cameraZoom;

        private bool _dragging = false;
        private PointF _mousePos = new PointF(float.NaN, float.NaN);

        private bool _devMode = false;
        private bool _mx = false;

        private bool _building = false;
        private InfrastructureBuilding _infBuild = null;

        private ListBox _detailsPanel = null;
        private Form _detailsForm = null;

        private bool _paused = false;

        // Images
        private Bitmap empty = Properties.Resources.empty;

        private Bitmap fence_h;
        private Bitmap fence_v;
        private Bitmap fence_lt;
        private Bitmap fence_rt;
        private Bitmap fence_lb;
        private Bitmap fence_rb;

        private Bitmap grass;
        private Bitmap bush;
        private Bitmap tree;

        private Bitmap gate_o;
        private Bitmap gate_c;

        private Bitmap road;

        private Bitmap canteen;
        private Bitmap pub;

        private Bitmap rollercoaster;
        private Bitmap carousel;
        private Bitmap ferris_wheel;

        private Bitmap wrench;

        private List<Bitmap> visitorSprites = null;
        private List<Bitmap> repairmanSprites = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Játékablak példányosítása.
        /// </summary>
        public View()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods

        #endregion

        #region Form event handlers

        /// <summary>
        /// Játékablak betöltésének eseménykezelője.
        /// </summary>
        private void Nezet_Load(Object sender, EventArgs e)
        {
            // adatelérés példányosítása
            _dataAccess = new DataAccess();

            // modell létrehozása és az eseménykezelők társítása
            _model = new GameModel(_dataAccess);
            //_model.TableChanged += new EventHandler<int[,]>(Game_TableChanged);
            _model.CashChanged += new EventHandler<int>(Game_CashChanged);
            _model.GameTimeChanged += new EventHandler<int>(Game_GameTimeChanged);
            _model.BuildingUnsuccessful += new EventHandler<string>(Game_BuildingUnsuccessful);
            _model.VisitorNumberChanged += new EventHandler<int>(Game_VisitorNumberChanged);
            _model.RepairmanNumberChanged += new EventHandler<int>(Game_repairmanNumberNumberChanged);

            // időzítő létrehozása
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(Timer_Tick);

            // csempeméret beállítása
            _tileSize = 64;
            _peopleSpeedMultiplier = 2;

            // kamera inicializálása
            _cameraPosition = new PointF();
            _cameraZoom = 1f;

            LoadImages();

            // nézet eseményeinek kezelése
            Paint += Renderer;
            FormClosing += View_FormClosing;

            MouseDown += View_MouseDown;
            MouseUp += View_MouseUp;

            KeyDown += View_KeyDown;


            _detailsForm = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                StartPosition = FormStartPosition.CenterScreen
            };
            _detailsPanel = new ListBox() { Dock = DockStyle.Fill };
            _detailsForm.Controls.Add(_detailsPanel);
            _detailsForm.Deactivate += _detailsForm_LostFocus;
            _detailsForm.FormClosing += _detailsForm_FormClosing;

            // új játék indítása
            _model.NewGame();

            _timer.Start();

            _paused = false;

            // játékciklus száljának beállítása
            _gameLoopWorker = new BackgroundWorker();
            _gameLoopWorker.WorkerSupportsCancellation = true;
            _gameLoopWorker.DoWork += _gameLoopWorker_DoWork;
            _gameLoopWorker.RunWorkerAsync();
        }

        private void _detailsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            (sender as Form).Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// Billentyű lenyomásának eseménykezelője
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P || e.KeyCode == Keys.B)
            {
                BuildingSettings(this, EventArgs.Empty);
            }

            if (e.KeyCode == Keys.C)
            {
                _cameraPosition = new PointF();
                _cameraZoom = 1f;
            }

            if (e.KeyCode == Keys.Q)
            {
                MenuExit_Click(this, EventArgs.Empty);
            }

            if (e.KeyCode == Keys.D && e.Modifiers == Keys.Control)
            {
                _devMode = !_devMode;
            }

            if (e.KeyCode == Keys.M)
            {
                if (_devMode)
                {
                    _mx = !_mx;
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                StopBuilding();
            }
        }

        /// <summary>
        /// Azonos épületek építésének befejezése
        /// </summary>
        private void StopBuilding()
        {
            _building = false;
            _infBuild = null;
            toolBuilding.Text = "";
        }

        /// <summary>
        /// Egérgomb felengedésének eseménykezelője
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.Default;

                _dragging = false;

                if (!float.IsNaN(_mousePos.X) || !float.IsNaN(_mousePos.Y))
                    _mousePos = new PointF(float.NaN, float.NaN);
            }
        }

        /// <summary>
        /// Egérgomb lenyomásának eseménykezelője
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mouseMapPos = ConvertMousePositionToMap();

                if (mouseMapPos.X >= 0 && mouseMapPos.X < _model.TableSizeX &&
                   mouseMapPos.Y >= 0 && mouseMapPos.Y < _model.TableSizeY)
                {
                    if (_model.Table[mouseMapPos.Y, mouseMapPos.X] == -8)
                        TicketBoothSettings(this, EventArgs.Empty);
                    else if (_model.Table[mouseMapPos.Y, mouseMapPos.X] >= 0 && !_building)
                    {
                        _detailsPanel.Items.Clear();
                        List<string> details = _model.GetDetails(mouseMapPos.X, mouseMapPos.Y);
                        for (int i = 0; i < details.Count; i++)
                        {
                            _detailsPanel.Items.Add(details[i]);
                        }

                        _detailsForm.Show();
                    }


                    if (_building)
                    {
                        _model.BuildingInProgress(_infBuild.Type,
                                                  mouseMapPos.X, mouseMapPos.Y,
                                                  _infBuild.Price, _infBuild.Maintenance, _infBuild.Upkeep,
                                                  _infBuild.BuildTime, _infBuild.BuildingLength, _infBuild.BuildingWidth,
                                                  _infBuild.Fee, _infBuild.Capacity, _infBuild.MinCapacity,
                                                  _infBuild.BuildingName, _infBuild.Duration, _infBuild.Effect);
                    }
                    else
                    {
                        if (_model.Table[mouseMapPos.Y, mouseMapPos.X] == -1)
                            BuildingSettings(this, EventArgs.Empty);
                    }
                }

            }
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.NoMove2D;

                _dragging = true;

                if (float.IsNaN(_mousePos.X) || float.IsNaN(_mousePos.Y))
                    _mousePos = new PointF(MousePosition.X, MousePosition.Y);
            }
        }

        /// <summary>
        /// Részletek panel bezárása, ha kikattintunk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _detailsForm_LostFocus(object sender, EventArgs e)
        {
            (sender as Form).Close();
        }

        /// <summary>
        /// Ablak bezárásának eseménykezelője
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_FormClosing(object sender, FormClosingEventArgs e)
        {
            _gameLoopWorker.CancelAsync();
        }

        /// <summary>
        /// Grafika kirajzolása
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Renderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low; // bilinear
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;

            g.SetClip(e.ClipRectangle);

            PointF mousePos = ConvertMousePositionToGameArea();

            // Ürítés és háttér kirajzolása
            g.Clear(Color.Black);

            // Kamera beállítása
            g.TranslateTransform(_cameraPosition.X, _cameraPosition.Y);

            int sizeX = _model.TableSizeX;
            int sizeY = _model.TableSizeY;

            // Játéktér kirazolása
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    Rectangle rect = new Rectangle(i * _tileSize, j * _tileSize, _tileSize, _tileSize);

                    // Alapértelmezett struktúra
                    switch (_model.Table[j, i])
                    {
                        case -1:
                            g.DrawImageUnscaled(empty, rect);
                            break;
                        case -2:
                            g.DrawImageUnscaled(fence_h, rect);
                            break;
                        case -3:
                            g.DrawImageUnscaled(fence_v, rect);
                            break;
                        case -4:
                            g.DrawImageUnscaled(fence_lt, rect);
                            break;
                        case -5:
                            g.DrawImageUnscaled(fence_rt, rect);
                            break;
                        case -6:
                            g.DrawImageUnscaled(fence_lb, rect);
                            break;
                        case -7:
                            g.DrawImageUnscaled(fence_rb, rect);
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (var building in _model.BuildingList)
            {
                Point p = new Point(building.Position[0] * _tileSize,
                                    building.Position[1] * _tileSize);
                Bitmap bmp = new Bitmap(_tileSize, _tileSize);
                switch (building.Name)
                {
                    case "Canteen":
                        bmp = (canteen.Clone() as Bitmap);
                        break;
                    case "Pub":
                        bmp = (pub.Clone() as Bitmap);
                        break;
                    case "Rollercoaster":
                        bmp = (rollercoaster.Clone() as Bitmap);
                        break;
                    case "Carousel":
                        bmp = (carousel.Clone() as Bitmap);
                        break;
                    case "Ferris Wheel":
                        bmp = (ferris_wheel.Clone() as Bitmap);
                        break;
                    case "Tree":
                        bmp = (tree.Clone() as Bitmap);
                        break;
                    case "Bush":
                        bmp = (bush.Clone() as Bitmap);
                        break;
                    case "Grass":
                        bmp = (grass.Clone() as Bitmap);
                        break;
                    case "Road":
                        bmp = (road.Clone() as Bitmap);
                        break;
                    default:
                        break;
                }
                g.DrawImageUnscaled(bmp, p);
                bmp.Dispose();

                if (building.State == Infrastructure.BuildingCondition.Broken ||
                   building.State == Infrastructure.BuildingCondition.UnderConstruction)
                {
                    g.DrawImageUnscaled(wrench, p);
                }
            }

            // Látogatók kirajzolása
            foreach (Visitor visitor in _model.Visitors)
            {
                if (visitor.State != Person.PersonCondition.Use)
                    g.DrawImageUnscaled(visitorSprites[visitor.SpriteID],
                                        visitor.ScreenPosition.X + visitor.ScreenOffset.X,
                                        visitor.ScreenPosition.Y + visitor.ScreenOffset.Y);
            }

            // Karbantartók kirajzolása
            foreach (Repairman repairman in _model.Repairmen)
            {
                g.DrawImageUnscaled(repairmanSprites[repairman.SpriteID],
                                    repairman.ScreenPosition.X + repairman.ScreenOffset.X,
                                    repairman.ScreenPosition.Y + repairman.ScreenOffset.Y);
            }

            g.DrawImageUnscaled(_model.isOpened ? gate_o : gate_c, (sizeX / 2 - 1) * _tileSize, (sizeY - 2) * _tileSize);

            // Kurzor kirajzolása
            int width = _infBuild == null ? 1 : _infBuild.BuildingWidth;
            int height = _infBuild == null ? 1 : _infBuild.BuildingLength;
            if (mousePos.X >= _tileSize && mousePos.X < _model.TableSizeX * _tileSize - _tileSize &&
                mousePos.Y >= _tileSize && mousePos.Y < _model.TableSizeY * _tileSize - _tileSize)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.White)), ((int)mousePos.X / _tileSize) * _tileSize,
                                ((int)mousePos.Y / _tileSize) * _tileSize, width * _tileSize, height * _tileSize);
            }

            if (_devMode)
            {
                PrintDevInfo(g);
            }
        }

        #endregion

        #region Game loop

        /// <summary>
        /// Játékciklust működtető szál
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _gameLoopWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            GameLoop();
        }

        /// <summary>
        /// Játékciklus
        /// </summary>
        private void GameLoop()
        {
            OnLoad();
            while (_gameLoopWorker.IsBusy)
            {
                if (!_paused)
                {
                    try
                    {
                        OnDraw();
                        //BeginInvoke((MethodInvoker)delegate { Refresh(); });
                        this.Invalidate();
                        OnUpdate();
                        Thread.Sleep(22);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Adatok betöltése a grafikus motor 
        /// elindítása előtt.
        /// </summary>
        private void OnLoad()
        {

        }

        /// <summary>
        /// Adatok frissítése a motor futása közben.
        /// </summary>
        private void OnUpdate()
        {
            if (_dragging && !float.IsNaN(_mousePos.X) && !float.IsNaN(_mousePos.Y))
            {
                PointF mouseDelta = new PointF(Cursor.Position.X - _mousePos.X, Cursor.Position.Y - _mousePos.Y);
                _cameraPosition = new PointF(_cameraPosition.X + mouseDelta.X, _cameraPosition.Y + mouseDelta.Y);

                _mousePos = new PointF(Cursor.Position.X, Cursor.Position.Y);
            }

            _model.StepPeople(_tileSize, _peopleSpeedMultiplier);
        }

        /// <summary>
        /// Egyéb rajzesemények.
        /// </summary>
        private void OnDraw()
        {
        }

        #endregion

        #region Game event handlers

        /// <summary>
        /// Pénz eseménykezelője.
        /// </summary>
        private void Game_CashChanged(Object sender, int e)
        {
            toolMoney.Text = e.ToString("g");
        }

        /// <summary>
        /// Idő eseménykezelője.
        /// </summary>
        private void Game_GameTimeChanged(Object sender, int e)
        {
            toolTime.Text = TimeSpan.FromSeconds(e).ToString("g");
            // játékidő frissítése
        }

        /// <summary>
        /// Látogatók eseménykezelője.
        /// </summary>
        private void Game_VisitorNumberChanged(Object sender, int e)
        {
            toolGuest.Text = e.ToString("g");
        }

        /// <summary>
        /// Karbantartók eseménykezelője.
        /// </summary>
        private void Game_repairmanNumberNumberChanged(Object sender, int e)
        {
            toolRepairman.Text = e.ToString("g");
        }

        /// <summary>
        /// Építő paletta eseménykezelője
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildingSettings(object sender, EventArgs e)
        {
            Point mouseMapPos = ConvertMousePositionToMap();

            InfrastructureBuilding b = new InfrastructureBuilding();
            // Csak a párbeszédablak számára!!!
            int mouseX = MousePosition.X > Width / 2 ? MousePosition.X - b.Width : MousePosition.X;
            int mouseY = MousePosition.Y > Height / 2 ? MousePosition.Y - b.Height : MousePosition.Y;
            b.Location = new Point(mouseX, mouseY);
            b.ShowDialog();

            if (b.BuildingIsOk)
            {
                _building = true;
                _infBuild = b;
                toolBuilding.Text = $"Building: {b.BuildingName}";
            }
        }

        /// <summary>
        /// Hiba eseménykezelője.
        /// </summary>
        private void Game_BuildingUnsuccessful(Object sender, string message)
        {
            MessageBox.Show("Building unsuccesful!" + Environment.NewLine +
                                   message, "Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Jegypénztár beállításainak eseménykezelője
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TicketBoothSettings(object sender, EventArgs e)
        {
            StopBuilding();

            TicketBooth tb = new TicketBooth(_model.EntranceFee, _model.PriceOfRepairman, _model.PriceOfRepairing, _model.isOpened, _model.Cash);
            tb.ParkOpened += _model.Opening;

            tb.RepairmanHired += _model.HireRepairman;
            //Error message

            if (tb.ShowDialog() == DialogResult.OK)
            {
                _model.EntranceFee = tb.EntranceFee;
            }
        }

        #endregion

        #region Menu event handlers

        /// <summary>
        /// Új játék eseménykezelője.
        /// </summary>
        private void MenuNewGame_Click(Object sender, EventArgs e)
        {
            toolSaveGame.Enabled = true;

            _model.NewGame();

            _timer.Start();
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void MenuLoadGame_Click(Object sender, EventArgs e)
        {
            openFileDialog.Filter = "Theme park game save (*.dbr)|*.dbr";
            _timer.Enabled = false;
            _paused = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.LoadGameAsync(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Loading game unsuccesful!" + Environment.NewLine +
                                    "Wrong path, or the directory cannot be written.", "Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            _timer.Enabled = true;
            _paused = false;
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void MenuSaveGame_Click(Object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Theme park game save (*.dbr)|*.dbr";
            _timer.Enabled = false;
            _paused = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    await _model.SaveGameAsync(saveFileDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Saving game unsuccessful!" + Environment.NewLine +
                                    "Wrong path, or the directory cannnot be written.", "Fail!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            _timer.Enabled = true;
            _paused = false;
        }

        /// <summary>
        /// Kilépés eseménykezelője.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuExit_Click(Object sender, EventArgs e)
        {
            Boolean restartTimer = _timer.Enabled;
            _timer.Stop();
            _paused = true;
            // megkérdezzük, hogy biztos ki szeretne-e lépni
            if (MessageBox.Show("Do you really want to exit?", "Theme park game", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // ha igennel válaszol
                Close();
            }
            else
            {
                if (restartTimer)
                {
                    _timer.Start();
                    _paused = false;
                }
            }
        }



        #endregion

        #region Timer event handlers

        /// <summary>
        /// Időzítő eseménykeztelője.
        /// </summary>
        private void Timer_Tick(Object sender, EventArgs e)
        {
            _model.TimeAdvanced();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Kurzorpozíciót kijelző koordinátákról játéktér koordinátákká konvertálja.
        /// </summary>
        /// <returns></returns>
        private PointF ConvertMousePositionToGameArea()
        {
            return new PointF((PointToClient(MousePosition).X - _cameraPosition.X) / _cameraZoom, (PointToClient(MousePosition).Y - _cameraPosition.Y) / _cameraZoom);
        }

        /// <summary>
        /// Kurzorpozíciót kijelző koordinátákról játéktábla koordinátákká konvertálja.
        /// </summary>
        /// <returns></returns>
        private Point ConvertMousePositionToMap()
        {
            PointF mousePos = ConvertMousePositionToGameArea();

            return new Point((int)mousePos.X / _tileSize, (int)mousePos.Y / _tileSize);
        }

        /// <summary>
        /// Fejleszői infók kiírása
        /// </summary>
        /// <param name="g"></param>
        private void PrintDevInfo(Graphics g)
        {
            Font font = new Font(FontFamily.GenericMonospace, 12f, FontStyle.Bold, GraphicsUnit.Point);
            StringBuilder devInfo = new StringBuilder("Devmode on\n");
            devInfo.AppendLine($"Number of buildings: {_model.BuildingList.Count}");
            devInfo.AppendLine($"Number of visitors: {_model.Visitors.Count}");
            devInfo.AppendLine($"Number of repairmen: {_model.Repairmen.Count}");
            devInfo.AppendLine($"Number of nodes: {_model.GraphNodeCount}");

            devInfo.AppendLine("--- Graph ---");
            devInfo.AppendLine($"- Pending items: {_model.GraphPendingWorkItemCount}");
            devInfo.AppendLine($"- Completed items: {_model.GraphCompletedWorkItemCount}");
            devInfo.AppendLine($"- Number of threads: {_model.GraphThreadCount}");

            if (_mx)
            {
                devInfo.AppendLine("--- Matrices ---");
                float[,] distances = _model.GraphDistancesMatrix;
                string header = "   |_";
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    header += i.ToString().PadLeft(3, '_') + "_";
                }
                devInfo.AppendLine(header);
                for (int i = 0; i < distances.GetLength(0); i++)
                {
                    string row = i.ToString().PadLeft(3, ' ') + "| ";
                    for (int j = 0; j < distances.GetLength(1); j++)
                    {
                        row += distances[i, j].ToString().PadLeft(3, ' ') + " ";
                    }
                    devInfo.AppendLine(row);
                }
            }

            devInfo.AppendLine("--- Building stats ---");

            foreach (var item in _model.BuildingList)
            {
                string line = $"{item.Name} - ";
                if (item is Building)
                {
                    line += $"Queue: {(item as Building).WaitingVisitors.Count}";
                }

                devInfo.AppendLine(line);
            }

            List<Point> graphPoints = _model.GraphNodePoints;
            foreach (var point in graphPoints)
            {
                g.FillRectangle(new SolidBrush(Color.Red), point.X * _tileSize, point.Y * _tileSize, _tileSize, _tileSize);
            }

            List<Tuple<Point, Point>> lines = _model.GraphLines;
            foreach (var line in lines)
            {
                g.DrawLine(new Pen(Color.Blue, 5f), line.Item1.X * _tileSize + (_tileSize / 2), line.Item1.Y * _tileSize + (_tileSize / 2),
                           line.Item2.X * _tileSize + (_tileSize / 2), line.Item2.Y * _tileSize + (_tileSize / 2));
            }

            for (int i = 0; i < graphPoints.Count; i++)
            {
                g.DrawString(i.ToString(), font, new SolidBrush(Color.Yellow), graphPoints[i].X * _tileSize, graphPoints[i].Y * _tileSize);
            }

            g.ResetTransform();
            g.DrawString(devInfo.ToString(), font, new SolidBrush(Color.White), new PointF(0, menusor.Height));
        }

        /// <summary>
        /// Pixelformátum cserélése jobb teljesítmény reményében
        /// </summary>
        /// <param name="img"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static Bitmap Resample(Image img, Size size)
        {
            var bmp = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(img, new Rectangle(Point.Empty, size));
            }
            return bmp;
        }

        /// <summary>
        /// Képek betöltése és inicializálása
        /// </summary>
        private void LoadImages()
        {
            empty = Properties.Resources.empty;

            fence_h = Properties.Resources.fence_h;
            fence_v = Properties.Resources.fence_v;
            fence_lt = Properties.Resources.fence_lt;
            fence_rt = Properties.Resources.fence_rt;
            fence_lb = Properties.Resources.fence_lb;
            fence_rb = Properties.Resources.fence_rb;

            grass = Properties.Resources.grass;
            bush = Properties.Resources.bush;
            tree = Properties.Resources.tree;

            gate_o = Properties.Resources.gate_o;
            gate_c = Properties.Resources.gate_c;

            road = Properties.Resources.road;

            canteen = Properties.Resources.canteen;
            pub = Properties.Resources.pub;

            rollercoaster = Properties.Resources.rollercoaster;
            carousel = Properties.Resources.carousel;
            ferris_wheel = Properties.Resources.ferris_wheel;

            wrench = Properties.Resources.wrench;

            visitorSprites = new List<Bitmap>();

            visitorSprites.Add(Resample(Properties.Resources.person1, Properties.Resources.person1.Size));
            visitorSprites.Add(Resample(Properties.Resources.person2, Properties.Resources.person2.Size));
            visitorSprites.Add(Resample(Properties.Resources.person2, Properties.Resources.person3.Size));
            visitorSprites.Add(Resample(Properties.Resources.person2, Properties.Resources.person4.Size));
            visitorSprites.Add(Resample(Properties.Resources.person2, Properties.Resources.person5.Size));
            visitorSprites.Add(Resample(Properties.Resources.person2, Properties.Resources.person6.Size));
            visitorSprites.Add(Resample(Properties.Resources.person2, Properties.Resources.person7.Size));
            visitorSprites.Add(Resample(Properties.Resources.person2, Properties.Resources.person8.Size));
            visitorSprites.Add(Resample(Properties.Resources.person2, Properties.Resources.person9.Size));
            visitorSprites.Add(Resample(Properties.Resources.person10, Properties.Resources.person10.Size));
            visitorSprites.Add(Resample(Properties.Resources.person11, Properties.Resources.person11.Size));
            visitorSprites.Add(Resample(Properties.Resources.person12, Properties.Resources.person12.Size));
            visitorSprites.Add(Resample(Properties.Resources.person13, Properties.Resources.person13.Size));
            visitorSprites.Add(Resample(Properties.Resources.person14, Properties.Resources.person14.Size));

            repairmanSprites = new List<Bitmap>();

            repairmanSprites.Add(Resample(Properties.Resources.mech1, Properties.Resources.mech1.Size));
            repairmanSprites.Add(Resample(Properties.Resources.mech2, Properties.Resources.mech2.Size));
            repairmanSprites.Add(Resample(Properties.Resources.mech3, Properties.Resources.mech3.Size));
            repairmanSprites.Add(Resample(Properties.Resources.mech4, Properties.Resources.mech4.Size));

            empty = Resample(empty, empty.Size);

            fence_h = Resample(fence_h, fence_h.Size);
            fence_v = Resample(fence_v, fence_v.Size);
            fence_lt = Resample(fence_lt, fence_lt.Size);
            fence_rt = Resample(fence_rt, fence_rt.Size);
            fence_lb = Resample(fence_lb, fence_lb.Size);
            fence_rb = Resample(fence_rb, fence_rb.Size);

            grass = Resample(grass, grass.Size);
            bush = Resample(bush, bush.Size);
            tree = Resample(tree, tree.Size);

            gate_o = Resample(gate_o, gate_o.Size);
            gate_c = Resample(gate_c, gate_c.Size);

            road = Resample(road, road.Size);

            canteen = Resample(canteen, canteen.Size);
            pub = Resample(pub, pub.Size);

            rollercoaster = Resample(rollercoaster, rollercoaster.Size);
            carousel = Resample(carousel, carousel.Size);
            ferris_wheel = Resample(ferris_wheel, ferris_wheel.Size);

            wrench = Resample(wrench, wrench.Size);
        }

        #endregion
    }
}
