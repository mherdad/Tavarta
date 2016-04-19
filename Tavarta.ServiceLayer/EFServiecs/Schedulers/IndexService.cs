﻿using Tavarta.DataLayer.Context;

namespace Tavarta.ServiceLayer.EFServiecs.Schedulers
{
    public class IndexService
    {
        public void AllIndexRebuild()
        {
            using (var _unitOfWork = new ApplicationDbContext())
            {
                _unitOfWork.Database.ExecuteSqlCommand(@"  IF OBJECT_ID('tempdb..#work_to_do') IS NOT NULL
        DROP TABLE tempdb..#work_to_do

BEGIN TRY
--BEGIN TRAN

-- Ensure a USE  statement has been executed first.

    SET NOCOUNT ON;

                DECLARE @objectid INT;
                DECLARE @indexid INT;
                DECLARE @partitioncount BIGINT;
                DECLARE @schemaname NVARCHAR(130);
                DECLARE @objectname NVARCHAR(130);
                DECLARE @indexname NVARCHAR(130);
                DECLARE @partitionnum BIGINT;
                DECLARE @partitions BIGINT;
                DECLARE @frag FLOAT;
                DECLARE @pagecount INT;
                DECLARE @command NVARCHAR(4000);

                DECLARE @page_count_minimum SMALLINT
                SET @page_count_minimum = 50

    DECLARE @fragmentation_minimum FLOAT
    SET @fragmentation_minimum = 30.0

-- Conditionally select tables and indexes from the sys.dm_db_index_physical_stats function
--and convert object and index IDs to names.

    SELECT object_id AS objectid,
           index_id AS indexid,
           partition_number AS partitionnum,
           avg_fragmentation_in_percent AS frag,
           page_count AS page_count
    INTO    #work_to_do
    FROM    sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL,
                                           'LIMITED')
    WHERE avg_fragmentation_in_percent > @fragmentation_minimum
            AND index_id > 0
            AND page_count > @page_count_minimum;

                IF CURSOR_STATUS('global', 'partitions') >= -1
BEGIN
 PRINT 'partitions CURSOR DELETED';
                CLOSE partitions
    DEALLOCATE partitions
END
-- Declare the cursor for the list of partitions to be processed.
    DECLARE partitions CURSOR LOCAL
    FOR
        SELECT *
        FROM    #work_to_do;

-- Open the cursor.
    OPEN partitions;

                --Loop through the partitions.
                   WHILE(1 = 1)
        BEGIN;
                FETCH NEXT
FROM partitions
INTO @objectid, @indexid, @partitionnum, @frag, @pagecount;

                IF @@FETCH_STATUS < 0
                BREAK;

                SELECT @objectname = QUOTENAME(o.name),
                        @schemaname = QUOTENAME(s.name)
            FROM sys.objects AS o
                 JOIN sys.schemas AS s ON s.schema_id = o.schema_id
            WHERE o.object_id = @objectid;

                SELECT @indexname = QUOTENAME(name)
            FROM sys.indexes
         WHERE   object_id = @objectid
                    AND index_id = @indexid;

                SELECT @partitioncount = COUNT(*)
            FROM sys.partitions
         WHERE   object_id = @objectid
                    AND index_id = @indexid;

                SET @command = N'ALTER INDEX ' + @indexname + N' ON '
                    + @schemaname + N'.' + @objectname + N' REBUILD';

                IF @partitioncount > 1
                SET @command = @command + N' PARTITION='
                    + CAST(@partitionnum AS NVARCHAR(10));

                EXEC(@command);
                --print(@command); //uncomment for testing

                PRINT N'Rebuilding index ' + @indexname + ' on table '
                    + @objectname;
                PRINT N'  Fragmentation: ' + CAST(@frag AS VARCHAR(15));
                PRINT N'  Page Count:    ' + CAST(@pagecount AS VARCHAR(15));
                PRINT N' ';
                END;

                --Close and deallocate the cursor.
    CLOSE partitions;
                DEALLOCATE partitions;

                --Drop the temporary table.
                   DROP TABLE #work_to_do;
--COMMIT TRAN

END TRY
BEGIN CATCH
--ROLLBACK TRAN
    PRINT 'ERROR ENCOUNTERED:' + ERROR_MESSAGE()
END CATCH");
            }
        }

        public IndexService()
        {
        }

        //public void ReBuildUserIndex()
        //{
        //    using (var _unitOfWork = new ApplicationDbContext())
        //    {
        //        _unitOfWork.Database.ExecuteSqlCommand("Alter index All on [dbo].[Users] rebuild;");
        //    }
        //}

        //public void ReBuildUserRoleIndex()
        //{
        //    using (var _unitOfWork = new ApplicationDbContext())
        //    {
        //        _unitOfWork.Database.ExecuteSqlCommand("Alter index All on [dbo].[UserRole] rebuild;");
        //    }
        //}

        //public void ReBuildUserLoginsIndex()
        //{
        //    using (var _unitOfWork = new ApplicationDbContext())
        //    {
        //        _unitOfWork.Database.ExecuteSqlCommand("alter index all on [dbo].[UserLogins] rebuild;");
        //    }
        //}

        //public void RebuildUserClaimIndex()
        //{
        //    using (var _unitOfWork = new ApplicationDbContext())
        //    {
        //        _unitOfWork.Database.ExecuteSqlCommand("alter index all on [dbo].[UserClaims] rebuild;");
        //    }
        //}

        //public void ReBuildRolesIndex()
        //{
        //    using (var _unitOfWork = new ApplicationDbContext())
        //    {
        //        _unitOfWork.Database.ExecuteSqlCommand("alter index all on [dbo].[Roles] rebuild;");
        //    }
        //}
    }
}