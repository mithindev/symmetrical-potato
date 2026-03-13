# TDS Feature Deployment Guide

This document summarizes the technical changes and deployment requirements for the Horizontal TDS Report, Multi-Branch Consolidation Hub, and Auto-Calculation features.

## 1. Database Schema
You must create the following table on your **Central Hub** database to store consolidated data from all branches.

```sql
CREATE TABLE TDS_Consolidated_Data (
    MemberNo VARCHAR(50) NOT NULL,
    FinancialYear VARCHAR(9) NOT NULL, -- Format: '2025-2026'
    FirstName VARCHAR(255),
    PAN VARCHAR(20),
    -- Monthly Interest Columns (Matches the pivot matrix)
    Apr_RD MONEY DEFAULT 0, Apr_FD MONEY DEFAULT 0, Apr_RID MONEY DEFAULT 0, Apr_SB MONEY DEFAULT 0, Apr_KMK MONEY DEFAULT 0, Apr_Total MONEY DEFAULT 0,
    May_RD MONEY DEFAULT 0, May_FD MONEY DEFAULT 0, May_RID MONEY DEFAULT 0, May_SB MONEY DEFAULT 0, May_KMK MONEY DEFAULT 0, May_Total MONEY DEFAULT 0,
    Jun_RD MONEY DEFAULT 0, Jun_FD MONEY DEFAULT 0, Jun_RID MONEY DEFAULT 0, Jun_SB MONEY DEFAULT 0, Jun_KMK MONEY DEFAULT 0, Jun_Total MONEY DEFAULT 0,
    Jul_RD MONEY DEFAULT 0, Jul_FD MONEY DEFAULT 0, Jul_RID MONEY DEFAULT 0, Jul_SB MONEY DEFAULT 0, Jul_KMK MONEY DEFAULT 0, Jul_Total MONEY DEFAULT 0,
    Aug_RD MONEY DEFAULT 0, Aug_FD MONEY DEFAULT 0, Aug_RID MONEY DEFAULT 0, Aug_SB MONEY DEFAULT 0, Aug_KMK MONEY DEFAULT 0, Aug_Total MONEY DEFAULT 0,
    Sep_RD MONEY DEFAULT 0, Sep_FD MONEY DEFAULT 0, Sep_RID MONEY DEFAULT 0, Sep_SB MONEY DEFAULT 0, Sep_KMK MONEY DEFAULT 0, Sep_Total MONEY DEFAULT 0,
    Oct_RD MONEY DEFAULT 0, Oct_FD MONEY DEFAULT 0, Oct_RID MONEY DEFAULT 0, Oct_SB MONEY DEFAULT 0, Oct_KMK MONEY DEFAULT 0, Oct_Total MONEY DEFAULT 0,
    Nov_RD MONEY DEFAULT 0, Nov_FD MONEY DEFAULT 0, Nov_RID MONEY DEFAULT 0, Nov_SB MONEY DEFAULT 0, Nov_KMK MONEY DEFAULT 0, Nov_Total MONEY DEFAULT 0,
    Dec_RD MONEY DEFAULT 0, Dec_FD MONEY DEFAULT 0, Dec_RID MONEY DEFAULT 0, Dec_SB MONEY DEFAULT 0, Dec_KMK MONEY DEFAULT 0, Dec_Total MONEY DEFAULT 0,
    Jan_RD MONEY DEFAULT 0, Jan_FD MONEY DEFAULT 0, Jan_RID MONEY DEFAULT 0, Jan_SB MONEY DEFAULT 0, Jan_KMK MONEY DEFAULT 0, Jan_Total MONEY DEFAULT 0,
    Feb_RD MONEY DEFAULT 0, Feb_FD MONEY DEFAULT 0, Feb_RID MONEY DEFAULT 0, Feb_SB MONEY DEFAULT 0, Feb_KMK MONEY DEFAULT 0, Feb_Total MONEY DEFAULT 0,
    Mar_RD MONEY DEFAULT 0, Mar_FD MONEY DEFAULT 0, Mar_RID MONEY DEFAULT 0, Mar_SB MONEY DEFAULT 0, Mar_KMK MONEY DEFAULT 0, Mar_Total MONEY DEFAULT 0,
    
    TotalInterest MONEY DEFAULT 0,
    
    CONSTRAINT PK_TDS_Consolidated PRIMARY KEY (MemberNo, FinancialYear)
);
```

## 2. Server Infrastructure
1.  **File System**: Create a folder named `tds` in the root of your ASP.NET application. Inside that, create a subfolder named `Processed`.
    -   `~/tds/`: Where you drop incoming `.dat` (BCP) files from branches.
    -   `~/tds/Processed/`: Where the system automatically archives files after a successful merge.
2.  **Permissions**: Ensure the IIS AppPool user has **Read/Write/Delete** permissions for the `~/tds/` directory.
3.  **BCP Utility**: The server must have SQL Server Command Line Utilities installed (`bcp.exe` must be in the System PATH or accessible).

## 3. Core Feature Logic

### TDS Auto-Calculation (The 5000 Rule)
TDS is calculated in-memory (VB.NET) just before data is bound to the UI or exported to CSV. 
-   **Threshold**: It accumulates interest monthly starting from April.
-   **Trigger**: Once the accumulator crosses **5000** in a financial year, it deducts 10% TDS on the accumulated balance.
-   **Continuous**: After crossing the threshold, it applies 10% TDS to all subsequent monthly earnings WITHOUT further threshold checks (as requested).

### Multi-Branch Consolidation
1.  **Export**: Individual branches use the `Export to .DAT` button to generate a BCP file containing their raw interest data.
2.  **Import**: The Central Hub uses a SQL `MERGE` statement. If a member already exists in the `TDS_Consolidated_Data` table for that year, the incoming interest amounts from the new file are **added** to any existing values, ensuring a mathematically accurate global total per member.

## 4. Deployment Check-List
1. [ ] Run the SQL script to create `TDS_Consolidated_Data`.
2. [ ] Create `~/tds/` and `~/tds/Processed/` folders on the web server.
3. [ ] Update [Web.config](file:///c:/Users/mithi/Desktop/Fiscus/Web.config) with the correct `fiscusdbConnectionString`.
4. [ ] Verify `bcp.exe` is installed on the server.
