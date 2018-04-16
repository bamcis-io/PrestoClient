namespace BAMCIS.PrestoClient.Model.SPI.Block
{
    /// <summary>
    /// From com.facebook.presto.spi.block.PageBuilderStatus.java
    /// </summary>
    public class PageBuilderStatus
    {
        #region Private Fields

        private bool _Full;

        private int _CurrentSize;

        #endregion

        #region Public Fields

        public static readonly int DEFAULT_MAX_PAGE_SIZE_IN_BYTES = 1024 * 1024;

        #endregion

        #region Public Properties

        public int MaxBlockSizeInBytes { get; }

        public int MaxPageSizeInBytes { get; }

        public bool Full
        {
            get
            {
                return this._Full || this.SizeInBytes >= this.MaxPageSizeInBytes;
            }
            set
            {
                this._Full = value;
            }
        }

        public int SizeInBytes
        {
            get
            {
                return this._CurrentSize;
            }
        }

        #endregion

        #region Constructors

        public PageBuilderStatus() : this(DEFAULT_MAX_PAGE_SIZE_IN_BYTES, BlockBuilderStatus.DEFAULT_MAX_BLOCK_SIZE_IN_BYTES)
        {
        }

        public PageBuilderStatus(int maxPageSizeInBytes, int maxBlockSizeInBytes)
        {
            this.MaxPageSizeInBytes = maxPageSizeInBytes;
            this.MaxBlockSizeInBytes = maxBlockSizeInBytes;
        }

        #endregion

        #region Public Methods

        public BlockBuilderStatus CreateBlockBuilderStatus()
        {
            return new BlockBuilderStatus(this, this.MaxBlockSizeInBytes);
        }

        public void AddBytes(int bytes)
        {
            this._CurrentSize += bytes;
        }

        public bool IsEmpty()
        {
            return this._CurrentSize == 0;
        }

        public override string ToString()
        {
            return StringHelper.Build(this)
                .Add("maxSizeInBytes", this.MaxPageSizeInBytes)
                .Add("full", this._Full)
                .Add("currentSize", this._CurrentSize)
                .ToString();
        }

        #endregion
    }
}
