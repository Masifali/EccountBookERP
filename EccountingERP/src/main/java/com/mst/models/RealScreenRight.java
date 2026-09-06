package com.mst.models;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;

import lombok.Data;

/**
 * READ-ONLY mapping of the real desktop's dbo.ScreenRights table - one row per
 * (screen, capability-name) combination it's possible to grant, e.g. for
 * ScreenDefinition.Id=9 (Chart Of Account): "View"=32480, "Save"=32481. Id is NOT a
 * small fixed enum - it's an app-assigned running counter across the whole real
 * table (tens of thousands of rows, one set per screen: View/Save/Update/Print/
 * "CanView AllRecord" and others), confirmed via goldenAce5_25t.sql.
 *
 * dbo.tblUserRights.RightId is a straight FK to this table's Id - see
 * {@link RealUserRight}. This port never writes to ScreenRights (it's real,
 * already-populated desktop data); it only reads the handful of rows needed to
 * resolve "the View right's Id" for whichever real screens {@link Screen}
 * (MstScreen) has been linked to via realScreenDefinitionId.
 */
@Entity
@Table(name = "ScreenRights")
@Data
public class RealScreenRight {

	@Id
	@Column(name = "Id")
	private Integer id;

	@Column(name = "RightName", length = 50)
	private String rightName;

	/** Real column is capitalized "ScreenID" (not "ScreenId") - see the real DDL. */
	@Column(name = "ScreenID")
	private Integer screenId;

	@Column(name = "Screens", length = 500)
	private String screens;
}
